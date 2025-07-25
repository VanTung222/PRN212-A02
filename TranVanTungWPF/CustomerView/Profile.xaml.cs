using System.Windows.Controls;
using CustomerVM;

namespace FUMiniHotelManagement.CustomerView
{
    public partial class Profile : UserControl
    {
        public Profile(FUMiniHotelSystem.DAL.Models.Customer customer)
        {
            InitializeComponent();
            DataContext = new ProfileVM(customer);
        }
    }
}