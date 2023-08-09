using System;
using System.Windows;
using System.Windows.Controls;
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

        public void OpenPopup(double minutesAllocated, double timeSpent)
        {
            ProblemLink.Text = "";
            Comments.Text = "";
            InputPopup.IsOpen = true;
            this.timeSpent = timeSpent;
            this.minutesAllocated = minutesAllocated;
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

            var newLine = DateTime.Now.ToString("d") + "," + ProblemLink.Text + "," + minutesAllocated + "," + timeSpent + "," + Status.SelectionBoxItem;

            if (!RetryingStreamWriter.Write(TrackingFilePath, DateTime.Now.ToString("d"), ProblemLink.Text, minutesAllocated, timeSpent, Status.SelectionBoxItem, Comments.Text))
            {
                Storyboard shakeAnimation = (Storyboard)FindResource("ShakeAnimation");
                ColorAnimation colorAnimation = (ColorAnimation)shakeAnimation.Children[1];
                var currentColor = colorAnimation.To;
                colorAnimation.To = Colors.Red;
                shakeAnimation.Begin(InputPopup);
                colorAnimation.To = currentColor;

                return;
            }

            InputPopup.IsOpen = false;
        }

        private void InputPopup_Opened(object sender, EventArgs e)
        {
            SetDefaultFocus();
        }

        private void SetDefaultFocus()
        {
            ProblemLink.Focus();
        }
    }
}
