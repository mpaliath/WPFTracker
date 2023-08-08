using System.Windows;
using System.Windows.Controls;
using WPFTracker.Utilities;
using WPFTracker.Windows.Timer;
using WPFTracker.Windows.Tracker;

namespace WPFTracker.Windows.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private TrackerActivityControl trackerUserControl;
        private TimedActivityControl timedActivityUserControl;
        private bool isHidden = false;

        public MainWindow()
        {
            InitializeComponent();
            trackerUserControl = new TrackerActivityControl();
            timedActivityUserControl = new TimedActivityControl();

            // Set the window startup location to manual
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            this.Loaded += MainWindow_Loaded;

            // Set the window style to none
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            // Set the window to always be on top
            this.Topmost = true;

            // Set the window size to match the size of the content
            // this.SizeToContent = SizeToContent.WidthAndHeight;

            ShowLastUsedWindow();

            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Adjust the size of the MainWindow to fit the content
            if (contentControl.Content is UserControl userControl)
            {
                this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
                //this.Top = SystemParameters.WorkArea.Height / 2;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the window position to the top right of the screen
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
            this.Top = 0;
        }

        private void ShowLastUsedWindow()
        {
            WindowSwitcher.SwitchToLastUsedWindow(trackerUserControl, timedActivityUserControl, contentControl);
        }

        private void ToggleSize_Click(object sender, RoutedEventArgs e)
        {
            if (trackerUserControl != null)
            {
                trackerUserControl.ToggleSize();
            }

            if (timedActivityUserControl != null)
            {
                timedActivityUserControl.ToggleSize();
            }

            isHidden = !isHidden;
            ToggleSize.Content = isHidden ? "<" : ">";

        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void SwitchModes_Click(object sender, RoutedEventArgs e)
        {
            WindowSwitcher.SwitchMode(trackerUserControl, timedActivityUserControl, contentControl);
        }
    }
}

