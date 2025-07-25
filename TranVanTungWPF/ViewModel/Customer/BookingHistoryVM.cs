using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using FUMiniHotelManagement.Utils;
using FUMiniHotelManagement.ViewModel.Customer;

namespace CustomerVM
{
    public class BookingHistoryVM : INotifyPropertyChanged
    {
        public ObservableCollection<BookingDetail> BookingDetailList { get; set; } = new();
        private List<BookingDetail> _allBookingDetails = new();

        public BookingDetail SelectedBooking { get; set; }
        public string SearchText { get; set; }

        public ICommand SearchCommand { get; }
        public ICommand ViewCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        private readonly BookingDetailService _bookingDetailService;
        private readonly BookingReservationService _bookingReservationService;
        private readonly int _customerId;

        public BookingHistoryVM(FUMiniHotelSystem.DAL.Models.Customer customer)
        {
            _customerId = customer.CustomerId;
            _bookingDetailService = new BookingDetailService(new FUMiniHotelSystem.DAL.BookingDetailRepository(new FUMiniHotelManagementContext()));
            _bookingReservationService = new BookingReservationService(new FUMiniHotelSystem.DAL.BookingReservationRepository(new FUMiniHotelManagementContext()));

            SearchCommand = new RelayCommand(_ => Search());
            ViewCommand = new RelayCommand(ViewBooking);
            EditCommand = new RelayCommand(EditBooking);
            DeleteCommand = new RelayCommand(DeleteBooking);

            LoadBookingDetails();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Load tất cả BookingDetail mà customer này là chủ nhân
        public async void LoadBookingDetails()
        {
            try
            {
                _allBookingDetails.Clear();
                BookingDetailList.Clear();

                var reservations = await _bookingReservationService.GetByCustomerAsync(_customerId);
                foreach (var res in reservations)
                {
                    var details = await _bookingDetailService.GetByBookingIdAsync(res.BookingReservationId);
                    foreach (var d in details)
                    {
                        _allBookingDetails.Add(d);
                        BookingDetailList.Add(d);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot load booking details: " + ex.Message);
            }
        }

        private void Search()
        {
            var keyword = (SearchText ?? "").ToLower();

            var filtered = _allBookingDetails.Where(detail =>
                (detail.Room?.RoomNumber ?? detail.RoomId.ToString()).ToLower().Contains(keyword)
                || detail.StartDate.ToString("dd/MM/yyyy").Contains(keyword)
                || detail.EndDate.ToString("dd/MM/yyyy").Contains(keyword)
                || ((detail.BookingReservation?.BookingDate.HasValue ?? false)
                    ? detail.BookingReservation.BookingDate.Value.ToString("dd/MM/yyyy")
                    : "").Contains(keyword)
                || (detail.ActualPrice?.ToString("N0") ?? "").Contains(keyword)
            ).ToList();

            BookingDetailList.Clear();
            foreach (var d in filtered)
                BookingDetailList.Add(d);
        }

        // CRUD
        private void ViewBooking(object obj)
        {
            var detail = obj as BookingDetail;
            if (detail == null) return;
            MessageBox.Show(
                $"Booking detail:\n" +
                $"BookingID: {detail.BookingReservationId}\n" +
                $"Room: {(detail.Room?.RoomNumber ?? detail.RoomId.ToString())}\n" +
                $"Check-in: {detail.StartDate:dd/MM/yyyy}\n" +
                $"Check-out: {detail.EndDate:dd/MM/yyyy}\n" +
                $"Price: {detail.ActualPrice:N0} VND");
        }

        private async void EditBooking(object obj)
        {
            var detail = obj as BookingDetail;
            if (detail == null) return;

            // Tạo ViewModel trung gian để edit
            var editVM = new EditBookingDetailVM
            {
                EditableDetail = new BookingDetail
                {
                    BookingReservationId = detail.BookingReservationId,
                    RoomId = detail.RoomId,
                    StartDate = detail.StartDate,
                    EndDate = detail.EndDate,
                    ActualPrice = detail.ActualPrice,
                    Room = detail.Room,
                    BookingReservation = detail.BookingReservation
                }
            };

            var editWindow = new FUMiniHotelManagement.CustomerView.EditBookingDetail
            {
                DataContext = editVM,
                Owner = Application.Current.MainWindow
            };

            if (editWindow.ShowDialog() == true)
            {
                var edited = editVM.EditableDetail;
                if (edited.StartDate > edited.EndDate)
                {
                    MessageBox.Show("Check-out date must be after check-in date!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Cập nhật vào database
                await _bookingDetailService.UpdateBookingDetailAsync(edited);

                // Reload lại data để mọi nơi đều cập nhật
                LoadBookingDetails();

                MessageBox.Show("Updated successfully!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private async void DeleteBooking(object obj)
        {
            var detail = obj as BookingDetail;
            if (detail == null) return;
            if (MessageBox.Show($"Are you sure to delete booking detail (BookingID: {detail.BookingReservationId}, RoomID: {detail.RoomId})?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                await _bookingDetailService.DeleteBookingDetailAsync(detail.BookingReservationId, detail.RoomId);
                LoadBookingDetails();
                MessageBox.Show("Booking detail deleted.");
            }
        }
    }
}
