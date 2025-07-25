using System.Windows;

namespace FUMiniHotelManagement.CustomerView
{
    public partial class EditBookingDetail : Window
    {
        public EditBookingDetail()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
