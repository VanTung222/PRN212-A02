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
using System.Windows.Shapes;

namespace FUMiniHotelManagement.CustomerView
{
    /// <summary>
    /// Interaction logic for CustomerControl.xaml
    /// </summary>
    public partial class CustomerControl : Window
    {
        public CustomerControl(FUMiniHotelSystem.DAL.Models.Customer customer)
        {
            InitializeComponent();
            this.DataContext = new CustomerVM.CustomerControlViewModel(customer);

        }
    }
}
