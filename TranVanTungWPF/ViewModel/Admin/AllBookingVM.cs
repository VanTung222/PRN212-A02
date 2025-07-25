using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using FUMiniHotelManagement.AdminView;

namespace AdminVM
{
    public class AllBookingVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Services
        private readonly IBookingDetailService _bookingDetailService;
        private readonly IBookingReservationService _bookingReservationService;
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;

        public ObservableCollection<BookingDetail> BookingList { get; set; } = new ObservableCollection<BookingDetail>();
        public ObservableCollection<RoomInformation> AllRooms { get; set; } = new ObservableCollection<RoomInformation>();
        public ObservableCollection<BookingReservation> AllReservations { get; set; } = new ObservableCollection<BookingReservation>();
        public ObservableCollection<Customer> AllCustomers { get; set; } = new ObservableCollection<Customer>();

        public BookingDetail SelectedBooking { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    _ = SearchBookingAsync();
                }
            }
        }

        private EditingBookingVM _editingBooking;
        public EditingBookingVM EditingBooking
        {
            get => _editingBooking;
            set { _editingBooking = value; OnPropertyChanged(nameof(EditingBooking)); }
        }

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SaveEditBookingCommand { get; }
        public ICommand CancelEditBookingCommand { get; }

        public AllBookingVM(IBookingDetailService bookingDetailService,
                           IBookingReservationService bookingReservationService,
                           ICustomerService customerService,
                           IRoomService roomService)
        {
            _bookingDetailService = bookingDetailService;
            _bookingReservationService = bookingReservationService;
            _customerService = customerService;
            _roomService = roomService;

            AddCommand = new RelayCommand(AddBooking);
            EditCommand = new RelayCommand(EditBooking, CanEditOrDelete);
            DeleteCommand = new RelayCommand(async _ => await DeleteBookingAsync(), CanEditOrDelete);
            SearchCommand = new RelayCommand(async _ => await SearchBookingAsync());
            SaveEditBookingCommand = new RelayCommand(SaveEditBooking);
            CancelEditBookingCommand = new RelayCommand(CancelEditBooking);

            _ = LoadAllBookingAsync();
            _ = LoadRoomsCustomersAndReservationsAsync();
        }

        // Load all Room, Customer, Reservation
        private async Task LoadRoomsCustomersAndReservationsAsync()
        {
            try
            {
                // Load rooms
                AllRooms.Clear();
                var rooms = await _roomService.GetAllRoomsAsync();
                foreach (var room in rooms)
                {
                    AllRooms.Add(room);
                }

                // Load customers
                AllCustomers.Clear();
                var customers = await _customerService.GetAllCustomersAsync();
                foreach (var customer in customers)
                {
                    AllCustomers.Add(customer);
                }

                // Load reservations
                AllReservations.Clear();
                var reservations = await _bookingReservationService.GetAllBookingReservationsAsync();
                foreach (var reservation in reservations)
                {
                    AllReservations.Add(reservation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddBooking(object obj)
        {
            try
            {
                EditingBooking = new EditingBookingVM();
                var dialog = new EditBooking { DataContext = this };

                if (dialog.ShowDialog() == true)
                {
                    // Validate input
                    if (EditingBooking.SelectedRoom == null)
                    {
                        MessageBox.Show("Please select a room!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(EditingBooking.CustomerName))
                    {
                        MessageBox.Show("Please enter customer name!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (EditingBooking.StartDate == null || EditingBooking.EndDate == null)
                    {
                        MessageBox.Show("Please select start and end dates!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (EditingBooking.StartDate >= EditingBooking.EndDate)
                    {
                        MessageBox.Show("End date must be after start date!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (EditingBooking.ActualPrice == null || EditingBooking.ActualPrice <= 0)
                    {
                        MessageBox.Show("Please enter valid actual price!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Find customer by name
                    var customerName = EditingBooking.CustomerName.Trim();
                    var customer = AllCustomers.FirstOrDefault(c =>
                        c.CustomerFullName.Trim().Equals(customerName, StringComparison.OrdinalIgnoreCase));

                    if (customer == null)
                    {
                        MessageBox.Show($"Customer '{customerName}' not found!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Find or create reservation for this customer
                    var reservation = AllReservations.FirstOrDefault(r => r.CustomerId == customer.CustomerId);
                    if (reservation == null)
                    {
                        // SOLUTION 1: Don't set ID, let database auto-generate
                        var newReservation = new BookingReservation
                        {
                            // BookingReservationId = 0, // Let database auto-generate
                            CustomerId = customer.CustomerId,
                            BookingDate = DateOnly.FromDateTime(DateTime.Now),
                            TotalPrice = EditingBooking.ActualPrice.Value,
                            BookingStatus = 1 // Active
                        };

                        // Add and get the generated reservation
                        var createdReservation = await _bookingReservationService.AddBookingReservationAsync(newReservation);
                        reservation = createdReservation;
                        
                        // Update local collection
                        AllReservations.Add(reservation);

                        if (reservation == null)
                        {
                            MessageBox.Show("Failed to create or find reservation!", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }

                    // Create new booking detail
                    var bookingDetail = new BookingDetail
                    {
                        RoomId = EditingBooking.SelectedRoom.RoomId,
                        BookingReservationId = reservation.BookingReservationId,
                        StartDate = DateOnly.FromDateTime(EditingBooking.StartDate.Value),
                        EndDate = DateOnly.FromDateTime(EditingBooking.EndDate.Value),
                        ActualPrice = EditingBooking.ActualPrice.Value
                    };

                    await _bookingDetailService.AddBookingDetailAsync(bookingDetail);

                    MessageBox.Show("Booking added successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    await LoadAllBookingAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add booking: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void EditBooking(object obj)
        {
            if (SelectedBooking == null) return;

            try
            {
                // Find the selected room for this booking
                var selectedRoom = AllRooms.FirstOrDefault(r => r.RoomId == SelectedBooking.RoomId);

                EditingBooking = new EditingBookingVM
                {
                    SelectedRoom = selectedRoom,
                    CustomerName = SelectedBooking.BookingReservation?.Customer?.CustomerFullName ?? "",
                    StartDate = SelectedBooking.StartDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = SelectedBooking.EndDate.ToDateTime(TimeOnly.MinValue),
                    ActualPrice = SelectedBooking.ActualPrice
                };

                var dialog = new EditBooking { DataContext = this };

                if (dialog.ShowDialog() == true)
                {
                    // Validate input
                    if (EditingBooking.SelectedRoom == null)
                    {
                        MessageBox.Show("Please select a room!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(EditingBooking.CustomerName))
                    {
                        MessageBox.Show("Please enter customer name!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (EditingBooking.StartDate == null || EditingBooking.EndDate == null)
                    {
                        MessageBox.Show("Please select start and end dates!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (EditingBooking.StartDate >= EditingBooking.EndDate)
                    {
                        MessageBox.Show("End date must be after start date!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (EditingBooking.ActualPrice == null || EditingBooking.ActualPrice <= 0)
                    {
                        MessageBox.Show("Please enter valid actual price!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Update booking details
                    SelectedBooking.RoomId = EditingBooking.SelectedRoom.RoomId;
                    SelectedBooking.StartDate = DateOnly.FromDateTime(EditingBooking.StartDate.Value);
                    SelectedBooking.EndDate = DateOnly.FromDateTime(EditingBooking.EndDate.Value);
                    SelectedBooking.ActualPrice = EditingBooking.ActualPrice.Value;

                    await _bookingDetailService.UpdateBookingDetailAsync(SelectedBooking);

                    MessageBox.Show("Booking updated successfully!", "Success",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    await LoadAllBookingAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update booking: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteBookingAsync()
        {
            if (SelectedBooking != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete booking of room \"{SelectedBooking.Room?.RoomNumber}\" for customer \"{SelectedBooking.BookingReservation?.Customer?.CustomerFullName}\"?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        await _bookingDetailService.DeleteBookingDetailAsync(
                            SelectedBooking.BookingReservationId, SelectedBooking.RoomId);

                        MessageBox.Show("Booking deleted successfully!", "Success",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        await LoadAllBookingAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to delete booking: {ex.Message}", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public async Task SearchBookingAsync()
        {
            try
            {
                BookingList.Clear();
                IEnumerable<BookingDetail> list;

                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    list = await _bookingDetailService.GetAllBookingDetailsAsync();
                }
                else
                {
                    list = (await _bookingDetailService.GetAllBookingDetailsAsync())
                        .Where(b =>
                            (b.Room != null && b.Room.RoomNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                            || (b.BookingReservation != null
                                && b.BookingReservation.Customer != null
                                && b.BookingReservation.Customer.CustomerFullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                        );
                }

                foreach (var item in list)
                    BookingList.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search failed: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadReservationsAsync()
        {
            AllReservations.Clear();
            var reservations = await _bookingReservationService.GetAllBookingReservationsAsync();
            foreach (var reservation in reservations)
            {
                AllReservations.Add(reservation);
            }
        }

        private void SaveEditBooking(object obj)
        {
            if (obj is Window window)
                window.DialogResult = true;
        }

        private void CancelEditBooking(object obj)
        {
            if (obj is Window window)
                window.DialogResult = false;
        }

        private async Task LoadAllBookingAsync()
        {
            try
            {
                BookingList.Clear();
                var list = await _bookingDetailService.GetAllBookingDetailsAsync();
                foreach (var item in list)
                    BookingList.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load bookings: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanEditOrDelete(object obj) => SelectedBooking != null;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}