using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FUMiniHotelSystem.DAL.Models;

namespace AdminVM
{
    public class EditUserVM : INotifyPropertyChanged
    {
        private Customer _customer;
        public EditUserVM(Customer customer)
        {
            // Clone or assign customer for editing (deep copy if needed)
            _customer = new Customer
            {
                CustomerId = customer.CustomerId,
                CustomerFullName = customer.CustomerFullName,
                Telephone = customer.Telephone,
                EmailAddress = customer.EmailAddress,
                CustomerBirthday = customer.CustomerBirthday,
                CustomerStatus = customer.CustomerStatus
            };
        }

        public Customer Customer
        {
            get => _customer;
            set { _customer = value; OnPropertyChanged(); }
        }

        public string CustomerFullName
        {
            get => _customer.CustomerFullName;
            set
            {
                if (_customer.CustomerFullName != value)
                {
                    _customer.CustomerFullName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Telephone
        {
            get => _customer.Telephone;
            set
            {
                if (_customer.Telephone != value)
                {
                    _customer.Telephone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EmailAddress
        {
            get => _customer.EmailAddress;
            set
            {
                if (_customer.EmailAddress != value)
                {
                    _customer.EmailAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? CustomerBirthday
        {
            get => _customer.CustomerBirthday?.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (value.HasValue)
                    _customer.CustomerBirthday = DateOnly.FromDateTime(value.Value);
                else
                    _customer.CustomerBirthday = null;
                OnPropertyChanged();
            }
        }

        public byte? CustomerStatus
        {
            get => _customer.CustomerStatus;
            set
            {
                if (_customer.CustomerStatus != value)
                {
                    _customer.CustomerStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
