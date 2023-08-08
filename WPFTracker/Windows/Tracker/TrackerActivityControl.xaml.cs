using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFTracker.Controls;
using WPFTracker.ViewModels;

using SystemTimer = System.Threading.Timer;

namespace WPFTracker.Windows.Tracker
{
    /// <summary>
    /// Interaction logic for TrackerActivityControl.xaml
    /// </summary>
    public partial class TrackerActivityControl : UserControl
    {

        private TrackerWindowViewModel viewModel;
        private SystemTimer resetTimer;

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

        public TrackerActivityControl()
        {
            InitializeComponent();
            SetupTimer();


            this.viewModel = new TrackerWindowViewModel();
            this.DataContext = this.viewModel;
            this.DataContextChanged += MainWindow_DataContextChanged;

            AppInfoPopup.OnPopupClosed += OnAppInfoPopupClosed;
            ContactInfoPopup.OnPopupClosed += ContactInfoPopup_OnPopupClosed;

        }



        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debugger.Break();
        }

        private void AppsFiledToday_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            AppInfoPopup.OpenPopup();

        }

        private void OnAppInfoPopupClosed(object? sender, EventArgs e)
        {
            if (e == EventArgs.Empty) { return; }

            var popupArgs = (PopupClosedEventArgs)e;
            if (popupArgs != null)
            {
                //FiledTodayCount++;
                //FiledTodayCount.Text = FiledTodayCount.ToString();

                PersistentTracker.Instance.TrackApp(popupArgs.Company, popupArgs.AppLink);
            }
        }

        private void AppsFiled_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ListPopup.IsOpen)
            {
                ListPopup.ClosePopup();
            }
            else
            {
                ListPopup.OpenPopup();
            }
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
