using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.BLL;

namespace FUMiniHotelManagement.AdminView
{
    /// <summary>
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : UserControl
    {
        public User()
        {
            InitializeComponent();
            this.DataContext = new AdminVM.UserVM(new CustomerService(new CustomerRepository(new FUMiniHotelSystem.DAL.Models.FUMiniHotelManagementContext())));
        }
    }
}
