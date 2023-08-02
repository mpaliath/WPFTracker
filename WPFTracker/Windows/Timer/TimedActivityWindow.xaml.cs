using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WPFTracker.Controls.TrackerTaskbarIcon;
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
        private bool isTimerCountingDown = false;

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
            var newContent = shouldRestart ? "Restart Timer" : "Stop Timer";
            if (!isTimerCountingDown)
            {
                if (TimerOptionsComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int minutes))
                {
                    remainingTime = TimeSpan.FromMinutes(minutes);
                    UpdateTimeLeft();
                }
                timer.Start();
                isTimerCountingDown = true;
                TimerAction.Content = newContent;
            }
            else
            {
                if (shouldRestart)
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
                else
                {
                    isTimerCountingDown = false;
                    timer.Stop();
                    TimerAction.Content = "Restart Timer";
                }
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isTimerCountingDown)
            {
                if (remainingTime.TotalSeconds > 0)
                {
                    if (remainingTime.TotalSeconds == 120)
                    {
                        SoundPlayer player = new SoundPlayer(Sounds.Notify);
                        player.Play();
                    }

                    remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
                    UpdateTimeLeft();
                }
                else
                {
                    isTimerCountingDown = false;
                    TimerAction.Content = "Stop Timer";
                    SoundPlayer player = new SoundPlayer(Sounds.Tada);
                    player.Play();
                }
            }
            else
            {
                remainingTime = remainingTime.Add(TimeSpan.FromSeconds(1));
                UpdateTimeLeft();
            }
        }

        Brush currentBrush = Brushes.Black;
        private void UpdateTimeLeft()
        {
            if (!isTimerCountingDown)
            {
                currentBrush = Brushes.DarkRed;
            }
            else if (TimerOptionsComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int minutes))
            {
                if (remainingTime.TotalMinutes == minutes && currentBrush != Brushes.DarkGreen)
                {
                    currentBrush = Brushes.DarkGreen;
                }
                else if (remainingTime.TotalMinutes <= minutes / 2 && currentBrush != Brushes.LightGreen)
                {
                    currentBrush = Brushes.LightGreen;
                }

                else if (remainingTime.TotalMinutes <= 2 && currentBrush != Brushes.Red)
                {
                    currentBrush = Brushes.Red;
                }
            }
            TimeLeftTextBlock.Foreground = currentBrush;
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

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                SetActionToRestart(true);
            }
        }

        private bool shouldRestart = false;
        private void SetActionToRestart(bool shouldRestart)
        {
            Dispatcher.Invoke(() =>
            {
                TimerAction.Content = "Restart Timer";
                shouldRestart = true;
            });
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                TimerAction.Content = "Stop Timer";
                shouldRestart = false;
            });
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            // Get the reference to the main window
            var mainWindow = Application.Current.MainWindow as TimedActivityWindow;

            // Minimize the window
            if (mainWindow != null)
            {
                TrackerTaskbarIcon.Instance.Initialize();

                mainWindow.Hide();
            }

        }
    }
}
