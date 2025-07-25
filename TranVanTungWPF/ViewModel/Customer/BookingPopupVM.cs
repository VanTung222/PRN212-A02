using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using FUMiniHotelManagement.Utils;

namespace CustomerVM
{
    public class BookingPopupVM : INotifyPropertyChanged
    {
        // Properties for displaying room info
        public string RoomNumber { get; set; }
        public string RoomTypeName { get; set; }
        public decimal RoomPricePerDay { get; set; }

        // Booking dates
        private DateTime? _checkInDate = DateTime.Now;
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set { _checkInDate = value; OnPropertyChanged(); }
        }

        private DateTime? _checkOutDate = DateTime.Now.AddDays(1);
        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set { _checkOutDate = value; OnPropertyChanged(); }
        }

        // Commands for Book and Cancel buttons
        public ICommand BookCommand { get; }
        public ICommand CancelCommand { get; }

        private readonly Customer _customer;
        private readonly RoomDTO _room;
        private readonly BookingReservationService _bookingService;
        private readonly BookingDetailService _bookingDetailService;
        private readonly Action _onBooked; // Optional: refresh room list in parent VM

        public BookingPopupVM(Customer customer, RoomDTO room, BookingReservationService bookingService, BookingDetailService bookingDetailService, Action onBooked)
        {
            _customer = customer;
            _room = room;
            _bookingService = bookingService;
            _bookingDetailService = bookingDetailService;
            _onBooked = onBooked;

            RoomNumber = room.RoomNumber;
            RoomTypeName = room.RoomTypeName;
            RoomPricePerDay = room.RoomPricePerDay;

            BookCommand = new RelayCommand(async _ => await BookRoom());
            CancelCommand = new RelayCommand(ClosePopup);
        }

        // Command handlers
        private void ClosePopup(object obj)
        {
            foreach (Window w in Application.Current.Windows)
            {
                if (w.DataContext == this)
                {
                    w.Close();
                    break;
                }
            }
        }

        private async System.Threading.Tasks.Task BookRoom()
        {
            if (CheckInDate == null || CheckOutDate == null || CheckInDate >= CheckOutDate)
            {
                MessageBox.Show("Please select a valid check-in and check-out date!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int days = (CheckOutDate.Value - CheckInDate.Value).Days;
            if (days <= 0)
            {
                MessageBox.Show("Check-out date must be after check-in date.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal totalPrice = days * RoomPricePerDay;

            var booking = new BookingReservation
            {
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                TotalPrice = totalPrice,
                CustomerId = _customer.CustomerId,
                BookingStatus = 1 // 1 = Booked (adapt as per your status enum)
            };

            try
            {
                var createdBooking = await _bookingService.AddBookingReservationAsync(booking);
                
                // Create the booking detail
                var bookingDetail = new BookingDetail
                {
                    RoomId = _room.RoomID,
                    BookingReservationId = createdBooking.BookingReservationId,
                    StartDate = DateOnly.FromDateTime(CheckInDate.Value),
                    EndDate = DateOnly.FromDateTime(CheckOutDate.Value),
                    ActualPrice = totalPrice
                };
                
                await _bookingDetailService.AddBookingDetailAsync(bookingDetail);
                
                MessageBox.Show("Booking successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Close the popup and trigger a refresh if provided
                ClosePopup(null);
                _onBooked?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while booking:\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
