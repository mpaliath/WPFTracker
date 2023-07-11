using Octokit;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for Feedback.xaml
    /// </summary>
    public partial class Feedback : UserControl
    {

        private string token = "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";
        public Feedback()
        {
            InitializeComponent();
        }

        public bool IsPopupOpen
        {
            get { return true; }
        }

        internal void OpenPopup()
        {
            var hintText = "Enter Issue Details...";
            FeedbackPopup.IsOpen = true;
            IssueDetails.Text = hintText;

        }


        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var client = new GitHubClient(new ProductHeaderValue("WPFTracker"));
            client.Credentials = new Credentials(token);

            var newIssue = new NewIssue(DateTime.Now.ToLocalTime().ToString("g"))
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
    }
}
