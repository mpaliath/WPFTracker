using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFTracker.Utilities;
using WPFTracker.Windows.Timer;
using WPFTracker.Windows.Tracker;
using WindowsPoint = System.Windows.Point;
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

            WindowSwitcher.SetFocus(contentControl);

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

        WindowsPoint initialMousePosition;
        double initialWindowTop;

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.LeftShift && e.IsUp == true)
            {

                this.Cursor = null;

                WindowSwitcher.SetFocus(contentControl);
                e.Handled = true;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed &&
                (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
            {
                var currentMousePosition = PointToScreen(e.GetPosition(this));
                Top = initialWindowTop + (currentMousePosition.Y - initialMousePosition.Y);
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift)
            {
                e.Handled = true;

                initialMousePosition = PointToScreen(e.GetPosition(this));
                initialWindowTop = Top;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Shift && Keyboard.FocusedElement is MainWindow)
            {
                this.Cursor = Cursors.SizeAll;
                e.Handled = true;
            }
        }
    }
}

