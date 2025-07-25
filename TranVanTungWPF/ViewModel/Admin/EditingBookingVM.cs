using System;
using System.ComponentModel;
using FUMiniHotelSystem.DAL.Models;

namespace AdminVM
{
    public class EditingBookingVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private RoomInformation _selectedRoom;
        public RoomInformation SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(nameof(SelectedRoom)); }
        }

        private string _customerName;
        public string CustomerName
        {
            get => _customerName;
            set { _customerName = value; OnPropertyChanged(nameof(CustomerName)); }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        private decimal? _actualPrice;
        public decimal? ActualPrice
        {
            get => _actualPrice;
            set { _actualPrice = value; OnPropertyChanged(nameof(ActualPrice)); }
        }

        protected void OnPropertyChanged(string prop) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
