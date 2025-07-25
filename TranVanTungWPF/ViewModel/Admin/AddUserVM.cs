using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FUMiniHotelSystem.DAL.Models;

namespace AdminVM
{
    public class AddUserVM : INotifyPropertyChanged
    {
        public string CustomerFullName { get => _customerFullName; set { _customerFullName = value; OnPropertyChanged(); } }
        public string Telephone { get => _telephone; set { _telephone = value; OnPropertyChanged(); } }
        public string EmailAddress { get => _emailAddress; set { _emailAddress = value; OnPropertyChanged(); } }
        public DateTime? CustomerBirthday { get => _customerBirthday; set { _customerBirthday = value; OnPropertyChanged(); } }
        public byte? CustomerStatus { get => _customerStatus; set { _customerStatus = value; OnPropertyChanged(); } }

        private string _customerFullName;
        private string _telephone;
        private string _emailAddress;
        private DateTime? _customerBirthday;
        private byte? _customerStatus = 1;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public Customer ToCustomer()
        {
            return new Customer
            {
                CustomerFullName = CustomerFullName,
                Telephone = Telephone,
                EmailAddress = EmailAddress,
                CustomerBirthday = CustomerBirthday.HasValue ? DateOnly.FromDateTime(CustomerBirthday.Value) : null,
                CustomerStatus = CustomerStatus
            };
        }
    }
}
