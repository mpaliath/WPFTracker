using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTracker.Controls
{

    public class PopupClosedEventArgs : EventArgs
    {
        internal PopupClosedEventArgs(string company, string appLink)
        {
            Company = company;
            AppLink = appLink;
        }

        public string Company { get; internal set; }
        public string AppLink { get; internal set; }
    }


    /// <summary>
    /// Interaction logic for AppInfo.xaml
    /// </summary>
    public partial class AppInfoControl : UserControl
    {
        public AppInfoControl()
        {
            InitializeComponent();
        }

        public void OpenPopup()
        {
            InputPopup.IsOpen = true;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the popup
            ClosePopup(new PopupClosedEventArgs(CompanyTextBox.Text, AppLinkTextBox.Text));
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape
                && string.IsNullOrEmpty(CompanyTextBox.Text)
                && string.IsNullOrEmpty(AppLinkTextBox.Text))
            {
                ClosePopup(EventArgs.Empty);
            }
            else if (e.Key == Key.Enter)
            {
                SubmitButton_Click(null, null);
            }
        }

        private void ClosePopup(EventArgs e)
        {
            InputPopup.IsOpen = false;
            OnPopupClosed?.Invoke(this, e);
        }

        private void InputPopup_Opened(object sender, EventArgs e)
        {
            CompanyTextBox.Focus(); // Set focus to the TextBox
        }

        public event EventHandler<EventArgs>? OnPopupClosed;
    }
}
