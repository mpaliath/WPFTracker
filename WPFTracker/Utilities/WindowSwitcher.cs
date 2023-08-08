using System.Windows;
using System.Windows.Controls;
using WPFTracker.Windows.Timer;
using WPFTracker.Windows.Tracker;

namespace WPFTracker.Utilities
{
    public static class WindowSwitcher
    {
        public static void SwitchToLastUsedWindow(TrackerActivityControl trackerUserControl, TimedActivityControl timedActivityUserControl, ContentControl contentControl)
        {
            if (App.AppSettings.ShouldSwitchMode)
                contentControl.Content = timedActivityUserControl;
            else
                contentControl.Content = trackerUserControl;

        }

        public static void SwitchMode(TrackerActivityControl trackerUserControl, TimedActivityControl timedActivityUserControl, ContentControl contentControl)
        {
            if (contentControl.Content is TimedActivityControl)
            {
                trackerUserControl.Visibility = Visibility.Visible;
                contentControl.Content = trackerUserControl;
                if (timedActivityUserControl != null && timedActivityUserControl.IsVisible)
                    timedActivityUserControl.Visibility = Visibility.Collapsed;

                App.AppSettings.ShouldSwitchMode = false;
                App.AppSettings.Save();
            }
            else
            {
                timedActivityUserControl.Visibility = Visibility.Visible;
                contentControl.Content = timedActivityUserControl;
                if (trackerUserControl != null && trackerUserControl.IsVisible)
                    trackerUserControl.Visibility = Visibility.Collapsed;

                App.AppSettings.ShouldSwitchMode = true;
                App.AppSettings.Save();
            }
        }

        private static Visibility SwitchContent(Visibility visibility)
        {
            if (visibility == Visibility.Visible)
                return Visibility.Collapsed;

            else
                return Visibility.Visible;
        }
    }
}
