using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using WPFTracker.Controls;
using WPFTracker.Utilities;
using WPFTracker.ViewModels;

namespace WPFTracker.Windows.Timer
{
    /// <summary>
    /// Interaction logic for TimedActivityControl.xaml
    /// </summary>
    public partial class TimedActivityControl : CollapsibleControl
    {
        private TimeSpan remainingTime = TimeSpan.Zero;
        private DispatcherTimer timer = new DispatcherTimer();
        private bool isTimerCountingDown = false;

        public TimedActivityControl() : base(new CollapsibleControlModel(new GridLength(1, GridUnitType.Star)))
        {
            InitializeComponent();
            SetupTimer();
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
            //TODO

        }
    }
}
