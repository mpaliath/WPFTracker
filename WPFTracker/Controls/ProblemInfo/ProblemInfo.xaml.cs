using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using WPFTracker.Utilities;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for ProblemInfo.xaml
    /// </summary>
    public partial class ProblemInfo : UserControl
    {
        private const string TrackingFilePath = "C:\\Users\\mahes\\source\\repos\\WPFTracker\\TrackedProblems.csv";
        private double timeSpent;
        private double minutesAllocated;

        public static readonly DependencyProperty AnimationColorProperty = DependencyProperty.Register("AnimationColor", typeof(Color), typeof(ProblemInfo), new PropertyMetadata(Colors.Red));

        public Color AnimationColor
        {
            get { return (Color)GetValue(AnimationColorProperty); }
            set { SetValue(AnimationColorProperty, value); }
        }

        public ProblemInfo()
        {
            InitializeComponent();
        }

        TaskCompletionSource popupTask;
        public Task OpenPopup(double minutesAllocated, double timeSpent, bool revisit)
        {
            popupTask = new TaskCompletionSource();

            InputPopup.IsOpen = true;
            this.timeSpent = timeSpent;
            this.minutesAllocated = minutesAllocated;

            if (revisit)
            {
                ShouldRedo.Visibility = Visibility.Visible;
                ShouldRedo.IsChecked = true;
            }
            else
            {
                ShouldRedo.Visibility = Visibility.Collapsed;
                ShouldRedo.IsChecked = false;
            }


            return popupTask.Task;
        }

        public void ClosePopup(EventArgs? e, bool clearFields = false)
        {
            InputPopup.IsOpen = false;

            if (clearFields)
            {
                ProblemLink.Text = "";
                Comments.Text = "";
            }

            if (clearFields)
            {
                popupTask.SetResult();
            }
            else
            {
                popupTask.SetCanceled();
            }
        }



        private void Submit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetDefaultFocus();
            if (ProblemLink.Text == string.Empty)
            {
                Storyboard shakeAnimation = (Storyboard)FindResource("ShakeAnimation");
                shakeAnimation.Begin(InputPopup);
                return;
            }

            if (!RetryingStreamWriter.Write(TrackingFilePath, DateTime.Now.ToString("d"), ProblemLink.Text, minutesAllocated, timeSpent, Status.SelectionBoxItem, Comments.Text, (bool)ShouldRedo.IsChecked ? "YES" : "NO"))
            {
                Storyboard shakeAnimation = (Storyboard)FindResource("ShakeAnimation");
                ColorAnimation colorAnimation = (ColorAnimation)shakeAnimation.Children[1];
                var currentColor = colorAnimation.To;
                colorAnimation.To = Colors.Red;
                shakeAnimation.Begin(InputPopup);
                colorAnimation.To = currentColor;

                return;
            }

            ClosePopup(null, true);
        }

        private void InputPopup_Opened(object sender, EventArgs e)
        {
            SetDefaultFocus();
        }

        private void SetDefaultFocus()
        {
            ProblemLink.Focus();
        }

        private void InputPopup_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape
                && string.IsNullOrEmpty(ProblemLink.Text)
                && string.IsNullOrEmpty(Comments.Text))
            {
                ClosePopup(null, false);
            }
            else if (e.Key == Key.Enter)
            {
                Submit_Click(null, null);
            }
        }
    }
}
