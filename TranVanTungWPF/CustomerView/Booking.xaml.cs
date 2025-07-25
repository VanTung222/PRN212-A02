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
using CustomerVM;

namespace FUMiniHotelManagement.CustomerView
{
 
    public partial class Booking : UserControl
    {
        public Booking(FUMiniHotelSystem.DAL.Models.Customer customer)
        {
            InitializeComponent();
            DataContext = new BookingVM(customer);
        }
    }
}
