using System.Windows;
using System.Windows.Controls;
using WPFTracker.Windows.Timer;

namespace WPFTracker.Utilities
{
    public static class WindowSwitcher
    {
        public static void SwitchToWindow(Window currentWindow, Window targetWindow)
        {
            if (currentWindow != null)
            {
                currentWindow.Close();
            }

            targetWindow.Show();

            //App.AppSettings.ShouldSwitchMode = targetWindow is TimedActivityWindow;
            App.AppSettings.Save();
        }

        public static void SwitchMode(UserControl trackerUserControl, UserControl timedActivityUserControl, ContentControl contentControl)
        {
            if (contentControl.Content is TimedActivityControl)
            {
                trackerUserControl.Visibility = Visibility.Visible;
                contentControl.Content = trackerUserControl;
                if (timedActivityUserControl != null && timedActivityUserControl.IsVisible)
                    timedActivityUserControl.Visibility = Visibility.Collapsed;

            }
            else
            {
                timedActivityUserControl.Visibility = Visibility.Visible;
                contentControl.Content = timedActivityUserControl;
                if (trackerUserControl != null && trackerUserControl.IsVisible)
                    trackerUserControl.Visibility = Visibility.Collapsed;
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
