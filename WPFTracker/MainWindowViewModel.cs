using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WPFTracker.Utilities;

namespace WPFTracker
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public static MainWindowViewModel Instance = new MainWindowViewModel();
        public MainWindowViewModel()
        {
            Logger.Instance.ExceptionLogged += OnExceptionLogged;
        }

        private void OnExceptionLogged(object sender, EventArgs e)
        {
            ShowBadge = Visibility.Visible;
        }

        private Visibility _showBadge = Visibility.Hidden;
        public Visibility ShowBadge
        {
            get { return _showBadge; }
            set
            {
                _showBadge = value;
                OnPropertyChanged("ShowBadge");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public PersistentTracker Tracker
        {
            get { return PersistentTracker.Instance; }
        }
    }
}
