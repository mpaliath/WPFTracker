using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTracker.Controls
{

    public class PopupClosedEventArgs : EventArgs
    {
        internal PopupClosedEventArgs(string company, string appLink, string designation)
        {
            Company = company;
            AppLink = appLink;
            Designation = designation;
        }

        public string Company { get; internal set; }
        public string AppLink { get; internal set; }
        public string Designation { get; }
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

        

        private void SubmitButton_Click(object? sender, RoutedEventArgs? e)
        {
            // Close the popup
            var args = new PopupClosedEventArgs(CompanyTextBox.Text, AppLinkTextBox.Text, DesignationComboBox.Text);
            ClosePopup(args);
            CompanyTextBox.Text = "";
            AppLinkTextBox.Text = string.Empty;
        }

        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && IsDirty)
            {
                SubmitButton_Click(null, null);
            }
        }

        public void ClosePopup(EventArgs e)
        {
            OnSubmit?.Invoke(this, e);
            
        }

        internal void InputPopup_Opened(object sender, EventArgs e)
        {
            CompanyTextBox.Focus(); // Set focus to the TextBox
        }

        public bool IsDirty {
            get { return !string.IsNullOrEmpty(CompanyTextBox.Text) || !string.IsNullOrEmpty(AppLinkTextBox.Text); }
                
        }

        public event EventHandler<EventArgs>? OnSubmit;

    }
}
