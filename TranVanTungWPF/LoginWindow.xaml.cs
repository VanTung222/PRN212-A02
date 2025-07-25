using System.Windows;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using Config;
using Microsoft.EntityFrameworkCore;
using FUMiniHotelManagement.AdminView;
using FUMiniHotelManagement.CustomerView;

namespace FUMiniHotelSystem
{
    public partial class LoginWindow : Window
    {
        private readonly ICustomerService _customerService;

        public LoginWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService(new CustomerRepository(new FUMiniHotelManagementContext()));
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string inputEmail = txtUser.Text.Trim();
            string inputPassword = txtPass.Password.Trim();

            string adminEmail = AppConfig.GetAdminEmail();
            string adminPassword = AppConfig.GetAdminPassword();

            if (inputEmail == adminEmail && inputPassword == adminPassword)
            {
                MessageBox.Show($"Hello {adminEmail}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                new AdminControl().Show();
                return;
            }

            Customer? customer = await _customerService.GetCustomerByEmailAsync(inputEmail);

            if (customer != null && customer.Password == inputPassword && customer.CustomerStatus == 1)
            {
                MessageBox.Show($"Welcome {customer.CustomerFullName}", "Customer", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Hide();
                new CustomerControl(customer).Show();
                return;
            }

            // 3. Thất bại
            MessageBox.Show("Invalid email or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}