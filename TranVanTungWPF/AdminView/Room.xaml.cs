using System.Windows.Controls;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;

namespace FUMiniHotelManagement.AdminView
{
    public partial class Room : UserControl
    {
        public Room()
        {
            InitializeComponent();
            DataContext = new AdminVM.RoomVM(
                new RoomService(new RoomRepository(new FUMiniHotelManagementContext())),
                new RoomTypeService(new RoomTypeRepository(new FUMiniHotelManagementContext()))
            );
        }
    }
}
