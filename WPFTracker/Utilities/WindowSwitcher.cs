using System.Windows;
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

            App.AppSettings.ShouldSwitchMode = targetWindow is TimedActivityWindow;
            App.AppSettings.Save();
        }
    }
}
