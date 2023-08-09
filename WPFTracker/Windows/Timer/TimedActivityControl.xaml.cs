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
            currentState = TimerState.Stopped;
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

        private void TimerAction_Click(object sender, RoutedEventArgs e)
        {
            ActionDefinition newAction = GetActionForState(currentState);

            if (TimerOptionsComboBox.SelectedItem is ComboBoxItem selectedItem && int.TryParse(selectedItem.Content.ToString(), out int minutes))
            {
                currentState = ProcessButtonClick(newAction, minutes);

                newAction = GetActionForState(currentState);
                TimerAction.Content = newAction.Text;

                if (newAction.Type != ActionType.Start)
                {
                    TimerOptionsComboBox.IsEnabled = false;
                }
                else
                {
                    TimerOptionsComboBox.IsEnabled = true;
                }

                if (currentState == TimerState.Stopped)
                {
                    var timespent = minutes - (isTimerCountingDown ? 1 : -1) * remainingTime.TotalMinutes;
                    ProblemInfoPopup.OpenPopup(minutes, timespent);
                }
            }



        }

        private ActionDefinition GetActionForState(TimerState state)
        {
            ActionDefinition newAction = ButtonActions.None;
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (currentState == TimerState.Started)
                    newAction = ButtonActions.Pause;
                else if (currentState == TimerState.Paused)
                    newAction = ButtonActions.Restart;
                else if (currentState == TimerState.Stopped)
                    newAction = ButtonActions.Start;
            }
            else
            {
                if (currentState == TimerState.Started)
                    newAction = ButtonActions.Stop;
                else if (currentState == TimerState.Paused)
                    newAction = ButtonActions.Continue;
                else if (currentState == TimerState.Stopped)
                    newAction = ButtonActions.Start;
            }


            return newAction;
        }


        private TimerState ProcessButtonClick(ActionDefinition newAction, int minutes)
        {
            if (newAction == ButtonActions.None)
            {
                return currentState;
            }

            TimerState newState = TimerState.Started;

            switch (newAction.Type)
            {
                case ActionType.Start:
                    isTimerCountingDown = true;
                    remainingTime = TimeSpan.FromMinutes(minutes);
                    UpdateTimeLeft();
                    timer.Start();
                    newState = TimerState.Started;

                    break;

                case ActionType.Stop:
                    timer.Stop();
                    UpdateTimeLeft();
                    newState = TimerState.Stopped;

                    break;

                case ActionType.Restart:

                    isTimerCountingDown = true;
                    // Start the timer with the selected minutes
                    remainingTime = TimeSpan.FromMinutes(minutes);
                    UpdateTimeLeft();
                    timer.Start();
                    newState = TimerState.Started;

                    break;

                case ActionType.Pause:
                    timer.Stop();
                    newState = TimerState.Paused;

                    break;

                case ActionType.Continue:
                    timer.Start();
                    newState = TimerState.Started;

                    break;

            }


            return newState;
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
            System.Windows.Application.Current.Shutdown();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            var nextAction = GetActionForState(currentState);
            SetButtonAction(nextAction);
        }

        private void SetButtonAction(ActionDefinition action)
        {
            Dispatcher.Invoke(() =>
            {
                TimerAction.Content = action.Text;
                shouldRestart = true;
            });
        }

        private bool shouldRestart = false;
        private TimerState currentState;

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            Dispatcher.Invoke(() =>
                {
                    var nextAction = GetActionForState(currentState);
                    SetButtonAction(nextAction);

                    shouldRestart = false;
                });
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            //TODO

        }

        public enum ActionType
        {
            Stop,
            Restart,
            Start,
            Pause,
            Continue
        }

        public enum TimerState
        {
            Stopped,
            Started,
            Paused
        }

        private class ActionDefinition
        {

            public string Text { get; set; }
            public ActionType Type { get; }

            public ActionDefinition(string text, ActionType type)
            {
                Text = text;
                Type = type;
            }
        }

        private static class ButtonActions
        {
            public static ActionDefinition Stop = new ActionDefinition("Stop Timer", ActionType.Stop);
            public static ActionDefinition Restart = new ActionDefinition("Restart Timer", ActionType.Restart);
            public static ActionDefinition Start = new ActionDefinition("Start Timer", ActionType.Start);
            public static ActionDefinition Pause = new ActionDefinition("Pause Timer", ActionType.Pause);
            public static ActionDefinition Continue = new ActionDefinition("Continue Timer", ActionType.Continue);
            internal static ActionDefinition None;
        }


    }
}
