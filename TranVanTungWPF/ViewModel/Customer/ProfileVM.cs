using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using System.Windows.Input;
using FUMiniHotelManagement.Utils;

namespace CustomerVM
{
    public class ProfileVM : INotifyPropertyChanged
    {
        private Customer _customer;
        public ICommand UpdateProfileCommand { get; }

        private readonly CustomerService _customerService;

        public ProfileVM(Customer customer)
        {
            Customer = customer;
            _customerService = new CustomerService(new CustomerRepository(new FUMiniHotelManagementContext()));

            UpdateProfileCommand = new RelayCommand(async _ =>
            {
                await UpdateCustomerAsync();
            });
        }
        public Customer Customer
        {
            get => _customer;
            set { _customer = value; OnPropertyChanged(); }
        }

        // Binding cho từng property nếu muốn update tự động lên UI
        public string CustomerFullName
        {
            get => Customer?.CustomerFullName ?? "";
            set
            {
                if (Customer != null && Customer.CustomerFullName != value)
                {
                    Customer.CustomerFullName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Telephone
        {
            get => Customer?.Telephone ?? "";
            set
            {
                if (Customer != null && Customer.Telephone != value)
                {
                    Customer.Telephone = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EmailAddress
        {
            get => Customer?.EmailAddress ?? "";
            set
            {
                if (Customer != null && Customer.EmailAddress != value)
                {
                    Customer.EmailAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        // Handling DateTime for nullable CustomerBirthday
        public DateTime? CustomerBirthday
        {
            get => Customer?.CustomerBirthday.HasValue == true
                ? Customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue)
                : null;
            set
            {
                if (Customer != null)
                {
                    if (value.HasValue)
                        Customer.CustomerBirthday = DateOnly.FromDateTime(value.Value);
                    else
                        Customer.CustomerBirthday = null;
                    OnPropertyChanged();
                }
            }
        }


        private async Task UpdateCustomerAsync()
        {
            try
            {
                await _customerService.UpdateCustomerAsync(Customer);
                System.Windows.MessageBox.Show("Profile updated successfully!", "Success", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Update failed: " + ex.Message, "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
