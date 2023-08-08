using System;
using System.Threading;
using System.Windows;
using WPFTracker.Utilities;

namespace WPFTracker.ViewModels
{
    internal class TrackerWindowViewModel : CollapsibleControlModel
    {
        public static TrackerWindowViewModel Instance = new TrackerWindowViewModel();
        private Timer timer;
        private bool testBadge = false;
        public TrackerWindowViewModel() : base(new GridLength(1, GridUnitType.Star))
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

        public PersistentTracker Tracker
        {
            get { return PersistentTracker.Instance; }
        }


    }
}
