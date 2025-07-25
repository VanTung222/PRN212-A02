using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Linq;
using TranVanTungWPF;
using FUMiniHotelManagement.CustomerView;
using FUMiniHotelManagement.Utils;

namespace CustomerVM
{
    public class BookingVM : INotifyPropertyChanged
    {
        // Properties for UI binding
        public ObservableCollection<RoomDTO> AvailableRooms { get; set; }
        public RoomDTO SelectedRoom { get; set; }

        public string SearchText { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand BookingCommand { get; }

        private readonly Customer _customer;
        private readonly RoomService _roomService;
        private readonly BookingReservationService _bookingService;
        private readonly BookingDetailService _bookingDetailService;
        private readonly RoomTypeService _roomTypeService;
        private Dictionary<int, string> _roomTypeDict = new();

        public BookingVM(Customer customer)
        {
            _customer = customer;
            _roomService = new RoomService(new RoomRepository(new FUMiniHotelManagementContext()));
            _bookingService = new BookingReservationService(new BookingReservationRepository(new FUMiniHotelManagementContext()));
            _bookingDetailService = new BookingDetailService(new BookingDetailRepository(new FUMiniHotelManagementContext()));
            _roomTypeService = new RoomTypeService(new RoomTypeRepository(new FUMiniHotelManagementContext()));

            AvailableRooms = new ObservableCollection<RoomDTO>();
            SearchCommand = new RelayCommand(async _ => await SearchRoom());
            BookingCommand = new RelayCommand(OpenBookingPopup);

            LoadAvailableRooms();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private async void LoadAvailableRooms()
        {
            var rooms = await _roomService.GetAvailableRoomsAsync();
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            _roomTypeDict = roomTypes.ToDictionary(rt => rt.RoomTypeId, rt => rt.RoomTypeName);

            AvailableRooms.Clear();
            foreach (var room in rooms)
            {
                AvailableRooms.Add(new RoomDTO
                {
                    RoomID = room.RoomId,
                    RoomNumber = room.RoomNumber,
                    RoomDetailDescription = room.RoomDetailDescription,
                    RoomMaxCapacity = room.RoomMaxCapacity ?? 0,
                    RoomTypeName = _roomTypeDict.GetValueOrDefault(room.RoomTypeId, string.Empty),
                    RoomStatus = room.RoomStatus ?? 0,
                    RoomPricePerDay = room.RoomPricePerDay ?? 0
                });
            }
        }

        private async Task SearchRoom()
        {
            var rooms = string.IsNullOrWhiteSpace(SearchText)
                ? await _roomService.GetAvailableRoomsAsync()
                : await _roomService.SearchRoomAsync(SearchText);

            // Also update the RoomType dictionary in case RoomType table changed (optional)
            var roomTypes = await _roomTypeService.GetAllRoomTypesAsync();
            _roomTypeDict = roomTypes.ToDictionary(rt => rt.RoomTypeId, rt => rt.RoomTypeName);

            AvailableRooms.Clear();
            foreach (var room in rooms)
            {
                AvailableRooms.Add(new RoomDTO
                {
                    RoomID = room.RoomId,
                    RoomNumber = room.RoomNumber,
                    RoomDetailDescription = room.RoomDetailDescription,
                    RoomMaxCapacity = room.RoomMaxCapacity ?? 0,
                    RoomTypeName = _roomTypeDict.GetValueOrDefault(room.RoomTypeId, string.Empty),
                    RoomStatus = room.RoomStatus ?? 0,
                    RoomPricePerDay = room.RoomPricePerDay ?? 0
                });
            }
        }

        private void OpenBookingPopup(object roomObj)
        {
            var room = roomObj as RoomDTO;
            if (room == null) return;

            // Create the ViewModel for the popup
            var popupVM = new BookingPopupVM(_customer, room, _bookingService, _bookingDetailService, LoadAvailableRooms);

            // Create the popup window and set its DataContext
            var popup = new FUMiniHotelManagement.CustomerView.BookingPopup
            {
                DataContext = popupVM,
                Owner = App.Current.MainWindow
            };

            popup.ShowDialog();
        }
    }
}
