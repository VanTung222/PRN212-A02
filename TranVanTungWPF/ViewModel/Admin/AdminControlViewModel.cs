using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FUMiniHotelManagement.AdminView;
using FUMiniHotelManagement.CustomerView;
using FUMiniHotelSystem;
using FUMiniHotelManagement.Utils;

namespace AdminVM
{
    internal class AdminControlViewModel : INotifyPropertyChanged
    {
        public ICommand ShowUserCommand { get; }
        public ICommand ShowRoomCommand { get; }
        public ICommand ShowAllBookingCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ExitCommand { get; }

        private FUMiniHotelSystem.DAL.Models.Customer _customer;
        public FUMiniHotelSystem.DAL.Models.Customer Customer
        {
            get => _customer;
            set { _customer = value; OnPropertyChanged(); }
        }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public AdminControlViewModel()
        {
            ShowUserCommand = new RelayCommand(_ => CurrentView = new User());
            ShowRoomCommand = new RelayCommand(_ => CurrentView = new Room());
            ShowAllBookingCommand = new RelayCommand(_ => CurrentView = new AdminAllBooking());
            LogoutCommand = new RelayCommand(_ =>
            {
                new LoginWindow().Show();
                foreach (Window win in Application.Current.Windows)
                    if (win is CustomerControl) win.Close();
            });
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            CurrentView = new User();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
