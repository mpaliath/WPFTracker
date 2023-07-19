using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using WPFTracker.Utilities;

namespace WPFTracker.Windows.Timer
{
    /// <summary>
    /// Interaction logic for TimedActivityWindow.xaml
    /// </summary>
    public partial class TimedActivityWindow : Window
    {
        private TimeSpan remainingTime = TimeSpan.Zero;
        private DispatcherTimer timer = new DispatcherTimer();
        public TimedActivityWindow()
        {
            InitializeComponent();

            SetupWindowLoad();

            SetupTimer();
        }

        private void SetupWindowLoad()
        {
            this.Loaded += TimedActivityWindow_Loaded;

            // Set the window startup location to manual
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            // Set the window style to none
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            // Set the window to always be on top
            this.Topmost = true;

            // Set the window size to match the size of the content
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void TimedActivityWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the window position to the top right of the screen
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
            this.Top = 0;
        }

        private void SetupTimer()
        {
            // Set up the timer to tick every second
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Set "30" as the default value in the ComboBox
            SetDefaultTimerValue();
        }

        private void SetDefaultTimerValue()
        {
            foreach (ComboBoxItem item in TimerOptionsComboBox.Items)
            {
                if (item.Content.ToString() == "30")
                {
                    TimerOptionsComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void StartTimer_Click(object sender, RoutedEventArgs e)
        {
            if (TimerOptionsComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int minutes))
            {
                // Start the timer with the selected minutes
                remainingTime = TimeSpan.FromMinutes(minutes);
                UpdateTimeLeft();
                timer.Start();

                TimerAction.Content = "Restart Timer";
            }
            else
            {
                MessageBox.Show("Please select a valid timer duration.");
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (remainingTime.TotalSeconds > 0)
            {
                remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimeLeft();
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Timer completed.");
            }
        }

        private void UpdateTimeLeft()
        {
            if (TimerOptionsComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int minutes))
            {
                if (remainingTime.TotalMinutes == minutes)
                {
                    TimeLeftTextBlock.Foreground = Brushes.Green;
                }
                else if (remainingTime.TotalMinutes <= minutes / 2)
                {
                    TimeLeftTextBlock.Foreground = Brushes.Yellow;
                }

                else if (remainingTime.TotalMinutes <= 2)
                {
                    TimeLeftTextBlock.Foreground = Brushes.Red;
                }
            }
            TimeLeftTextBlock.Text = remainingTime.ToString();
        }

        private void TimerOptionsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimerOptionsComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int minutes))
            {
                remainingTime = TimeSpan.FromMinutes(minutes);
                UpdateTimeLeft();
            }
        }

        private void SwitchModes_Click(object sender, RoutedEventArgs e)
        {
            var window = new TrackerWindow();
            WindowSwitcher.SwitchToWindow(this, window);
        }

        private void Quit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
