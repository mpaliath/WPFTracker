using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTracker.Controls
{

    public class NewAppInfoEventArgs : EventArgs
    {
        internal NewAppInfoEventArgs(string company, string appLink, string designation)
        {
            Company = company;
            AppLink = appLink;
            Designation = designation;
        }

        public string Company { get; internal set; }
        public string AppLink { get; internal set; }
        public string Designation { get; }
    }

    public delegate void OnNewAppInfo(NewAppInfoEventArgs info);

    /// <summary>
    /// Interaction logic for AppInfo.xaml
    /// </summary>
    public partial class AppInfoControl : UserControl, IAcceptInput
    {
        public AppInfoControl()
        {
            InitializeComponent();
        }

        public event OnNewAppInfo OnNewApp;

        

        private void SubmitButton_Click(object? sender, RoutedEventArgs? e)
        {
            // Close the popup
            var args = new NewAppInfoEventArgs(CompanyTextBox.Text, AppLinkTextBox.Text, DesignationComboBox.Text);
            OnNewApp(args);
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

        public void OnClosing(object sender, EventArgs e)
        {
            OnSubmit?.Invoke(this, e);
            
        }

        public void OnOpened(object sender, EventArgs e)
        {
            CompanyTextBox.Focus(); // Set focus to the TextBox
        }

        public bool IsDirty {
            get { return !string.IsNullOrEmpty(CompanyTextBox.Text) || !string.IsNullOrEmpty(AppLinkTextBox.Text); }
                
        }

        public event EventHandler<EventArgs>? OnSubmit;

    }
}
