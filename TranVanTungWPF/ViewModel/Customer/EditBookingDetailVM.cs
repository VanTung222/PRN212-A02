using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelManagement.ViewModel.Customer
{
    public class EditBookingDetailVM : INotifyPropertyChanged
    {
        public BookingDetail EditableDetail { get; set; }

        public DateTime StartDateProxy
        {
            get => new DateTime(EditableDetail.StartDate.Year, EditableDetail.StartDate.Month, EditableDetail.StartDate.Day);
            set
            {
                EditableDetail.StartDate = DateOnly.FromDateTime(value);
                OnPropertyChanged();
            }
        }

        public DateTime EndDateProxy
        {
            get => new DateTime(EditableDetail.EndDate.Year, EditableDetail.EndDate.Month, EditableDetail.EndDate.Day);
            set
            {
                EditableDetail.EndDate = DateOnly.FromDateTime(value);
                OnPropertyChanged();
            }
        }

        public decimal? ActualPrice
        {
            get => EditableDetail.ActualPrice;
            set
            {
                EditableDetail.ActualPrice = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
