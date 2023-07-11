using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for AppsList.xaml
    /// </summary>
    public partial class AppsList : UserControl
    {
        public AppsList()
        {
            InitializeComponent();
            //dataGrid.ItemsSource = PersistentTracker.Instance.jobs;
        }

        public void OpenPopup()
        {
            ListPopup.IsOpen = true;

            dataGrid.Focus();
        }

        public bool IsOpen { get { return ListPopup.IsOpen; } }

        public void ClosePopup()
        {
            ListPopup.IsOpen = false;
        }


        public void ListPopup_Opened(object sender, EventArgs e)
        { }

        private void ListPopup_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape && ListPopup.IsOpen == true)

            {
                ClosePopup();
            }
        }

        public PersistentTracker Tracker => PersistentTracker.Instance;
    }
}
