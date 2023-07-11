using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTracker.Controls
{

    public class ContactPopupClosedEventArgs : EventArgs
    {
        internal ContactPopupClosedEventArgs(string name, string company)
        {
            Company = company;
            Name = name;
        }

        public string Company { get; internal set; }
        public string Name { get; internal set; }
    }


    /// <summary>
    /// Interaction logic for AppInfo.xaml
    /// </summary>
    public partial class ContactInfoControl : UserControl
    {
        public ContactInfoControl()
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
            ClosePopup(new ContactPopupClosedEventArgs(NameTextBox.Text, CompanyTextBox.Text));
            CompanyTextBox.Text = "";
            NameTextBox.Text = string.Empty;
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape
                && string.IsNullOrEmpty(CompanyTextBox.Text)
                && string.IsNullOrEmpty(NameTextBox.Text))
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
            NameTextBox.Focus(); // Set focus to the TextBox
        }

        public event EventHandler<EventArgs>? OnPopupClosed;
    }
}
