using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using WPFTracker.Utilities;

namespace WPFTracker.ViewModels
{
    internal class TrackerWindowViewModel : INotifyPropertyChanged
    {
        public static TrackerWindowViewModel Instance = new TrackerWindowViewModel();
        private Timer timer;
        private bool testBadge = false;
        public TrackerWindowViewModel()
        {
            Logger.Instance.ExceptionLogged += OnExceptionLogged;

            if (testBadge)
            {
                TimerCallback callback = (state) => { ShowBadge = Visibility.Visible; };
                timer = new Timer(callback, null, TimeSpan.FromSeconds(10), Timeout.InfiniteTimeSpan);
            }
        }

        private void OnExceptionLogged(object sender, EventArgs e)
        {
            ShowBadge = Visibility.Visible;
        }

        private Visibility _showBadge = Visibility.Collapsed;
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
