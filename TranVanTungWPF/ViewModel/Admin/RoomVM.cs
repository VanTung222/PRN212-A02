using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelSystem.DAL;
using FUMiniHotelSystem.DAL.Models;
using FUMiniHotelSystem.BLL;
using FUMiniHotelManagement.AdminView;
using FUMiniHotelManagement.Utils;

namespace AdminVM
{
    public class RoomVM : INotifyPropertyChanged
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<RoomInformation> Rooms { get; set; } = new ObservableCollection<RoomInformation>();
        public ObservableCollection<RoomType> RoomTypes { get; set; } = new ObservableCollection<RoomType>();
        public RoomInformation SelectedRoom { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    SearchCommand.Execute(null);
                }
            }
        }

        private string _searchText;

        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }

        public RoomVM(IRoomService roomService, IRoomTypeService roomTypeService)
        {
            _roomService = roomService;
            _roomTypeService = roomTypeService;
            AddCommand = new RelayCommand(AddRoom);
            EditCommand = new RelayCommand(EditRoom, CanEditOrDelete);
            DeleteCommand = new RelayCommand(DeleteRoom, CanEditOrDelete);
            SearchCommand = new RelayCommand(async _ => await SearchRoomAsync());

            _ = LoadRoomsAsync();
            _ = LoadRoomTypesAsync();
        }

        private async Task LoadRoomsAsync()
        {
            Rooms.Clear();
            var list = await _roomService.GetAllRoomsAsync();
            foreach (var r in list)
                Rooms.Add(r);
        }

        private async Task LoadRoomTypesAsync()
        {
            RoomTypes.Clear();
            var list = await _roomTypeService.GetAllRoomTypesAsync();
            foreach (var rt in list)
                RoomTypes.Add(rt);
        }

        private async Task SearchRoomAsync()
        {
            Rooms.Clear();
            var list = string.IsNullOrWhiteSpace(SearchText)
                ? await _roomService.GetAllRoomsAsync()
                : await _roomService.SearchRoomAsync(SearchText);

            foreach (var r in list)
                Rooms.Add(r);
        }

        private async void AddRoom(object obj)
        {
            var vm = new EditRoomVM(null, RoomTypes);
            var win = new EditRoom
            {
                Title = "Add Room",
                DataContext = vm
            };
            if (win.ShowDialog() == true)
            {
                var newRoom = vm.GetRoom();
                try
                {
                    await _roomService.AddRoomAsync(newRoom);
                    MessageBox.Show("Room added successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add room!\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                await LoadRoomsAsync();
            }
        }

        private async void EditRoom(object obj)
        {
            if (SelectedRoom != null)
            {
                var vm = new EditRoomVM(SelectedRoom, RoomTypes);
                var win = new EditRoom
                {
                    DataContext = vm
                };
                if (win.ShowDialog() == true)
                {
                    var updatedRoom = vm.GetRoom();
                    try
                    {
                        await _roomService.UpdateRoomAsync(updatedRoom);
                        MessageBox.Show("Room updated successfully!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update room!\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    await LoadRoomsAsync();
                }
            }
        }

        private async void DeleteRoom(object obj)
        {
            if (SelectedRoom != null)
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete room \"{SelectedRoom.RoomNumber}\"?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _roomService.DeleteRoomAsync(SelectedRoom.RoomId);
                    await LoadRoomsAsync();
                }
            }
        }

        private bool CanEditOrDelete(object obj) => SelectedRoom != null;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}