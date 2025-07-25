using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FUMiniHotelManagement.CustomerView;
using FUMiniHotelSystem;
using FUMiniHotelManagement.Utils;

namespace CustomerVM
{
    public class CustomerControlViewModel : INotifyPropertyChanged
    {
        public ICommand ShowProfileCommand { get; }
        public ICommand ShowBookingCommand { get; }
        public ICommand ShowHistoryCommand { get; }
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

        // Constructor nhận Customer
        public CustomerControlViewModel(FUMiniHotelSystem.DAL.Models.Customer customer)
        {
            Customer = customer;

            ShowProfileCommand = new RelayCommand(_ => CurrentView = new Profile(Customer));
            ShowBookingCommand = new RelayCommand(_ => CurrentView = new Booking(Customer));
            ShowHistoryCommand = new RelayCommand(_ => CurrentView = new BookingHistory(Customer));
            LogoutCommand = new RelayCommand(_ =>
            {
                new LoginWindow().Show();
                foreach (Window win in Application.Current.Windows)
                    if (win is CustomerControl) win.Close();
            });
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());

            CurrentView = new Profile(Customer);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }


}
