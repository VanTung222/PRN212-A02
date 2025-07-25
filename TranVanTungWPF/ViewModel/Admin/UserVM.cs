using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using FUMiniHotelManagement.Utils;

namespace AdminVM
{
    public class UserVM : INotifyPropertyChanged
    {
        private readonly ICustomerService _customerService;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Customer> Customers { get; set; } = new ObservableCollection<Customer>();
        public Customer SelectedCustomer { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    SearchCommand.Execute(null);
                }
            }
        }
        private string _searchText;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }

        public UserVM(ICustomerService customerService)
        {
            _customerService = customerService;
            AddCommand = new RelayCommand(AddCustomer);
            EditCommand = new RelayCommand(EditCustomer, CanEditOrDelete);
            DeleteCommand = new RelayCommand(DeleteCustomer, CanEditOrDelete);
            SearchCommand = new RelayCommand(async _ => await SearchCustomerAsync());

            _ = LoadCustomersAsync();
        }

        private async Task LoadCustomersAsync()
        {
            Customers.Clear();
            var list = await _customerService.GetAllCustomersAsync();
            foreach (var cus in list)
                Customers.Add(cus);
        }

        private async Task SearchCustomerAsync()
        {
            Customers.Clear();
            var list = string.IsNullOrWhiteSpace(SearchText)
                ? await _customerService.GetAllCustomersAsync()
                : await _customerService.SearchCustomersByNameAsync(SearchText);

            foreach (var cus in list)
                Customers.Add(cus);
        }

        private async void AddCustomer(object obj)
        {
            var addUserWindow = new FUMiniHotelManagement.AdminView.AddUser();
            bool? result = addUserWindow.ShowDialog();

            if (result == true)
            {
                var newCustomer = addUserWindow.NewCustomer;

                // Gọi service để thêm mới
                await _customerService.AddCustomerAsync(newCustomer);
                await LoadCustomersAsync();
                System.Windows.MessageBox.Show("User added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private async void EditCustomer(object obj)
        {
            if (SelectedCustomer != null)
            {
                var editWindow = new FUMiniHotelManagement.AdminView.EditUser(SelectedCustomer);
                bool? result = editWindow.ShowDialog();

                if (result == true)
                {
                    var editedCustomer = editWindow.EditedCustomer;
                    await _customerService.UpdateCustomerAsync(editedCustomer); // Gọi update ở đây
                    await LoadCustomersAsync();
                    System.Windows.MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }



        private async void DeleteCustomer(object obj)
        {
            if (SelectedCustomer != null)
            {
                var result = System.Windows.MessageBox.Show(
                    $"Are you sure you want to delete user \"{SelectedCustomer.CustomerFullName}\"?",
                    "Confirm Delete",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (result == System.Windows.MessageBoxResult.Yes)
                {
                    await _customerService.DeleteCustomerAsync(SelectedCustomer.CustomerId);
                    await LoadCustomersAsync();
                }
            }
        }


        private bool CanEditOrDelete(object obj) => SelectedCustomer != null;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
