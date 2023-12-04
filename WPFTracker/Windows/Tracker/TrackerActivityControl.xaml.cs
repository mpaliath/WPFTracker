using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPFTracker.Controls;
using WPFTracker.ViewModels;

using SystemTimer = System.Threading.Timer;

namespace WPFTracker.Windows.Tracker
{
    /// <summary>
    /// Interaction logic for TrackerActivityControl.xaml
    /// </summary>
    public partial class TrackerActivityControl : CollapsibleControl
    {

        private TrackerWindowViewModel viewModel;
        private AppInfoControl appInfoControl;
        private AppsList appsListControl;
        private SystemTimer resetTimer;
        private bool isCollapsed;


        private TimeSpan GetTimeUntilMidnight()
        {
            DateTime now = DateTime.Now;
            DateTime midnight = now.Date.AddDays(1); // Next midnight
            TimeSpan timeUntilMidnight = midnight - now;

            return timeUntilMidnight;
        }

        private void SetupTimer()
        {
            TimeSpan timeUntilMidnight = GetTimeUntilMidnight();

            resetTimer = new SystemTimer(_ =>
            {
                PersistentTracker.Instance.RefreshTracker();
                SetupTimer(); // Set up the timer again for the next midnight
            }, null, timeUntilMidnight, Timeout.InfiniteTimeSpan);
        }

        public TrackerActivityControl() : base(new TrackerWindowViewModel())
        {
            InitializeComponent();
            SetupTimer();


            this.viewModel = (TrackerWindowViewModel)this.DataContext;

            this.appInfoControl = new AppInfoControl();
            appInfoControl.OnNewApp += OnAppInfoPopupClosed;

            this.appsListControl = new AppsList();
            InputPopupControl.OnPopupClosing += InputPopupControl_OnPopupClosing;


            //AppInfoPopup.OnPopupClosed += OnAppInfoPopupClosed;
            ContactInfoPopup.OnPopupClosed += ContactInfoPopup_OnPopupClosed;

        }

        private bool InputPopupControl_OnPopupClosing()
        {
            PersistentTracker.Instance.UpdateApps();
            return true;
        }

        private void OnAppsListClosed(object arg1, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private void AppsFiledToday_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            InputPopupControl.OpenPopup(this.appInfoControl);
        }

        private void OnAppInfoPopupClosed(NewAppInfoEventArgs info)
        {
            //FiledTodayCount++;
            //FiledTodayCount.Text = FiledTodayCount.ToString();

            PersistentTracker.Instance.TrackApp(info.Company, info.AppLink, info.Designation);
        }

        private void AppsFiled_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            InputPopupControl.OpenPopup(this.appsListControl);
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemQuestion && Keyboard.Modifiers == ModifierKeys.Control)
            {

                FeedbackPopup.OpenPopup();
            }
        }

        private void VendorsGrid_OnMouseDown(Object sender, MouseButtonEventArgs e)
        {
            ContactInfoPopup.OpenPopup();
        }

        private void ContactInfoPopup_OnPopupClosed(object? sender, EventArgs e)
        {
            if (e == EventArgs.Empty) { return; }

            var popupArgs = (ContactPopupClosedEventArgs)e;
            if (popupArgs != null)
            {
                //FiledTodayCount++;
                //FiledTodayCount.Text = FiledTodayCount.ToString();

                PersistentTracker.Instance.TrackVendor(popupArgs.Name, popupArgs.Company);
            }
        }

        private void TotalVendorsGrid_OnMouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (VendorsPopup.IsOpen)
            {
                VendorsPopup.ClosePopup();
            }
            else
            {
                VendorsPopup.OpenPopup();
            }
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
