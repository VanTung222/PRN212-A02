using System.Windows;
using FUMiniHotelSystem.DAL.Models;
using AdminVM;

namespace FUMiniHotelManagement.AdminView
{
    public partial class EditUser : Window
    {
        public EditUser(Customer customer)
        {
            InitializeComponent();
            DataContext = new EditUserVM(customer);
        }

        public Customer EditedCustomer => ((EditUserVM)DataContext).Customer;

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = (EditUserVM)this.DataContext;
            if (string.IsNullOrWhiteSpace(vm.CustomerFullName) || string.IsNullOrWhiteSpace(vm.EmailAddress))
            {
                MessageBox.Show("Full Name and Email cannot be empty!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
