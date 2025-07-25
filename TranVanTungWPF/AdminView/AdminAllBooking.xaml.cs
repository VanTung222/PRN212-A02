using System.Windows.Controls;
using AdminVM;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;

namespace FUMiniHotelManagement.AdminView
{
    public partial class AdminAllBooking : UserControl
    {
        public AdminAllBooking()
        {
            InitializeComponent();
            var bookingDetailService = new BookingDetailService(new BookingDetailRepository(new FUMiniHotelManagementContext()));
            var bookingReservationService = new BookingReservationService(new BookingReservationRepository(new FUMiniHotelManagementContext()));
            var customerService = new CustomerService(new CustomerRepository(new FUMiniHotelManagementContext()));
            var roomService = new RoomService(new RoomRepository(new FUMiniHotelManagementContext()));

            this.DataContext = new AdminVM.AllBookingVM(
                bookingDetailService,
                bookingReservationService,
                customerService,
                roomService
            );
        }
    }
}
