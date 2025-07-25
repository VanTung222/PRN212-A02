using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using FUMiniHotelSystem.DAL.Models;

namespace AdminVM
{
    public class EditRoomVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RoomInformation _room;

        public ObservableCollection<RoomType> RoomTypes { get; }
        public Dictionary<byte, string> RoomStatuses { get; } = new Dictionary<byte, string>
        {
            { 0, "Available" },
            { 1, "Occupied" },
            { 2, "Cleaning" },
            { 3, "Maintenance" }
        };

        public EditRoomVM(RoomInformation room, ObservableCollection<RoomType> roomTypes)
        {
            if (room != null)
            {
                // Deep copy để tránh ảnh hưởng tới item gốc khi hủy
                _room = new RoomInformation
                {
                    RoomId = room.RoomId,
                    RoomNumber = room.RoomNumber,
                    RoomDetailDescription = room.RoomDetailDescription,
                    RoomMaxCapacity = room.RoomMaxCapacity,
                    RoomTypeId = room.RoomTypeId,
                    RoomStatus = room.RoomStatus,
                    RoomPricePerDay = room.RoomPricePerDay
                };
            }
            else
            {
                _room = new RoomInformation();
            }
            RoomTypes = roomTypes;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        // Các property binding tới XAML
        public string RoomNumber
        {
            get => _room.RoomNumber;
            set { _room.RoomNumber = value; OnPropertyChanged(nameof(RoomNumber)); }
        }
        public string RoomDetailDescription
        {
            get => _room.RoomDetailDescription;
            set { _room.RoomDetailDescription = value; OnPropertyChanged(nameof(RoomDetailDescription)); }
        }
        public int? RoomMaxCapacity
        {
            get => _room.RoomMaxCapacity;
            set { _room.RoomMaxCapacity = value; OnPropertyChanged(nameof(RoomMaxCapacity)); }
        }
        public int RoomTypeId
        {
            get => _room.RoomTypeId;
            set { _room.RoomTypeId = value; OnPropertyChanged(nameof(RoomTypeId)); }
        }
        public byte? RoomStatus
        {
            get => _room.RoomStatus;
            set { _room.RoomStatus = value; OnPropertyChanged(nameof(RoomStatus)); }
        }
        public decimal? RoomPricePerDay
        {
            get => _room.RoomPricePerDay;
            set { _room.RoomPricePerDay = value; OnPropertyChanged(nameof(RoomPricePerDay)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save(object obj)
        {
            if (obj is System.Windows.Window win) win.DialogResult = true;
        }
        private void Cancel(object obj)
        {
            if (obj is System.Windows.Window win) win.DialogResult = false;
        }

        public RoomInformation GetRoom() => _room;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Nếu chưa có RelayCommand, thêm vào đây
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);
        public void Execute(object parameter) => _execute(parameter);
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
