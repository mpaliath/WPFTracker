using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPFTracker.Controls.TrackerTaskbarIcon
{
    internal class TrackerTaskbarIcon
    {
        public static TrackerTaskbarIcon Instance = new TrackerTaskbarIcon();
        private TrackerTaskbarIcon() { }
        bool isTaskbarIconActive = false;
        private TaskbarIcon taskbarIcon;

        public void Initialize()
        {
            if (!isTaskbarIconActive)
            {
                InitializeTaskbarIcon();
            }
        }

        private void InitializeTaskbarIcon()
        {
            taskbarIcon = new TaskbarIcon();
            taskbarIcon.Icon = new System.Drawing.Icon(@"Assets\Icon.ico");

            // Create a custom popup
            var popup = new Popup();
            popup.AllowsTransparency = true;
            popup.StaysOpen = false;

            // Create a stack panel to hold the menu items
            var stackPanel = new StackPanel
            {
                Background = Brushes.White
            };

            // Add menu items to the stack panel

            var menuItem = new MenuItem
            {
                Header = "Restore",
            };
            menuItem.Click += (sender, e) =>
            {
                popup.IsOpen = false;
                Application.Current.MainWindow.Show();
            };
            stackPanel.Children.Add(menuItem);

            menuItem = new MenuItem
            {
                Header = "Exit"
            };
            menuItem.Click += (sender, e) =>
            {
                popup.IsOpen = false;
                // Perform any cleanup or save operations if needed
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    Application.Current.Shutdown();
                });
            };
            stackPanel.Children.Add(menuItem);

            // Set the stack panel as the content of the popup
            popup.Child = stackPanel;

            // Set the popup as the content of the TrayPopup
            taskbarIcon.TrayPopup = popup;

            // Handle the TrayMouseUp event to display the popup
            taskbarIcon.TrayLeftMouseDown += (sender, e) =>
            {
                var taskbarIconElement = (TaskbarIcon)sender;
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null)
                {

                    popup.HorizontalOffset += 120;
                    popup.IsOpen = true;
                }
            };

            taskbarIcon.TrayPopupOpen += (sender, e) =>
            {
                var cursorPosition = new Point();
                WinApi.GetCursorPos(ref cursorPosition);

                Popup f = (Popup)(sender as TaskbarIcon).TrayPopup;
                f.HorizontalOffset += 120;
            };

            taskbarIcon.Visibility = Visibility.Visible;
            isTaskbarIconActive = true;
        }
    }
}
