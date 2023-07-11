using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WPFTracker.Controls;

namespace WPFTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskbarIcon taskbarIcon;
        private MainWindowViewModel viewModel;

        private void InitializeTaskbarIcon()
        {
            taskbarIcon = new TaskbarIcon();
            taskbarIcon.Icon = new System.Drawing.Icon(@"Assets\Icon.ico");

            // Create a custom popup
            var popup = new Popup();
            popup.AllowsTransparency = true;
            popup.StaysOpen = false;



            // Create a stack panel to hold the menu items
            var stackPanel = new StackPanel();
            stackPanel.Background = Brushes.White;

            // Add menu items to the stack panel
            var menuItem1 = new System.Windows.Controls.MenuItem();
            menuItem1.Header = "Exit";
            menuItem1.Click += (sender, e) =>
            {
                // Perform any cleanup or save operations if needed
                System.Windows.Application.Current.Shutdown();
            };
            stackPanel.Children.Add(menuItem1);

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
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeTaskbarIcon();

            ShowInTaskbar = false;


            this.viewModel = new MainWindowViewModel();
            this.DataContext = this.viewModel;
            this.DataContextChanged += MainWindow_DataContextChanged;

            // Set the window startup location to manual
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            this.Loaded += MainWindow_Loaded;

            // Set the window style to none
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;

            // Set the window to always be on top
            this.Topmost = true;

            // Set the window size to match the size of the content
            this.SizeToContent = SizeToContent.WidthAndHeight;

            AppInfoPopup.OnPopupClosed += OnAppInfoPopupClosed;
            ContactInfoPopup.OnPopupClosed += ContactInfoPopup_OnPopupClosed;

        }



        private void MainWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Debugger.Break();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Set the window position to the top right of the screen
            this.Left = SystemParameters.WorkArea.Width - this.ActualWidth;
            this.Top = 0;
        }

        private void AppsFiledToday_OnMouseDown(object sender, MouseButtonEventArgs e)
        {

            AppInfoPopup.OpenPopup();

        }

        private void OnAppInfoPopupClosed(object? sender, EventArgs e)
        {
            if (e == EventArgs.Empty) { return; }

            var popupArgs = (PopupClosedEventArgs)e;
            if (popupArgs != null)
            {
                //FiledTodayCount++;
                //FiledTodayCount.Text = FiledTodayCount.ToString();

                PersistentTracker.Instance.TrackApp(popupArgs.Company, popupArgs.AppLink);
            }
        }

        private void AppsFiled_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (ListPopup.IsOpen)
            {
                ListPopup.ClosePopup();
            }
            else
            {
                ListPopup.OpenPopup();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.OemQuestion && Keyboard.Modifiers == ModifierKeys.Control)
            {

                FeedbackPopup.OpenPopup();
            }
        }

        private void VendorsGrid_OnMouseDown(Object sender, MouseButtonEventArgs e)
        {
            ContactInfoPopup.OpenPopup();
        }

        private void ContactInfoPopup_OnPopupClosed(object? sender, EventArgs e)
        {
            if (e == EventArgs.Empty) { return; }

            var popupArgs = (ContactPopupClosedEventArgs)e;
            if (popupArgs != null)
            {
                //FiledTodayCount++;
                //FiledTodayCount.Text = FiledTodayCount.ToString();

                PersistentTracker.Instance.TrackVendor(popupArgs.Name, popupArgs.Company);
            }
        }

        private void TotalVendorsGrid_OnMouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (VendorsPopup.IsOpen)
            {
                VendorsPopup.ClosePopup();
            }
            else
            {
                VendorsPopup.OpenPopup();
            }
        }
    }


}
