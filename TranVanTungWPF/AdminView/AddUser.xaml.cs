using System.Windows;
using AdminVM;
using FUMiniHotelSystem.DAL.Models;

namespace FUMiniHotelManagement.AdminView
{
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
            DataContext = new AddUserVM();
        }

        public Customer NewCustomer => ((AddUserVM)DataContext).ToCustomer();

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var vm = (AddUserVM)this.DataContext;
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
