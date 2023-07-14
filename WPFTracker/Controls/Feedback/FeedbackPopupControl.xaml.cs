using Octokit;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class FeedbackPopupControl : UserControl
    {

        private string token = "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";
        public FeedbackPopupControl()
        {
            InitializeComponent();
        }

        public bool IsPopupOpen
        {
            get { return true; }
        }

        internal void OpenPopup()
        {
            FeedbackPopup.IsOpen = true;
            IssueTitle.Focus();

        }

        internal void ClosePopup()
        {
            FeedbackPopup.IsOpen = false;
        }


        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var client = new GitHubClient(new ProductHeaderValue("WPFTracker"));
            client.Credentials = new Credentials(token);

            if (IssueTitle.Text == "" || IssueDetails.Text == "")
            {
                MessageBox.Show("Please enter the required fields",
                    "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }

            var newIssue = new NewIssue(IssueTitle.Text)
            {
                Body = IssueDetails.Text
            };

            Issue issue = null;
            try
            {
                issue = await client.Issue.Create("mpaliath", "WPFTracker", newIssue);
            }
            catch (Exception ex)
            {
                Debugger.Break();
            }

            MessageBox.Show($"Issue created - {issue.Number}",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            FeedbackPopup.IsOpen = false;
        }

        private void FeedbackPopup_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape && FeedbackPopup.IsOpen == true)

            {
                ClosePopup();
            }
        }
    }
}
