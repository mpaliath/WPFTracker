using System;
using System.Windows;
using System.Windows.Input;
using WPFTracker.Controls;

namespace WPFTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();

            //DataContext = PersistentTracker.Instance;

            // Set the window startup location to manual
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            this.Loaded += MainWindow_Loaded;

            // Set the window style to none
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            // Set the window to always be on top
            this.Topmost = true;

            // Set the window size to match the size of the content
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the window position to the top right of the screen
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
            this.Top = 0;

            var currentCounts = PersistentTracker.Instance.CountApps();

            //FiledTodayCount = currentCounts.appsToday;
            //FiledTodayCount.Text = FiledTodayCount.ToString();

            //FiledAppsCount = currentCounts.totalApps;
            //FiledAppsCount.Text = FiledAppsCount.ToString();
        }

        private void TextBlock1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AppInfoPopup.OnPopupClosed += OnAppInfoPopupClosed;
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

        private void TextBlock2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //FiledAppsCount++;
            //FiledAppsCount.Text = FiledAppsCount.ToString();
        }

    }


}
