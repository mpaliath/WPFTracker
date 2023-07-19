using System.Windows;

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
        }
    }
}
