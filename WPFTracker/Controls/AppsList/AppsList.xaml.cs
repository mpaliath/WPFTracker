using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFTracker.Data;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for AppsList.xaml
    /// </summary>
    public partial class AppsList : UserControl, IAcceptInput
    {
        public AppsList()
        {
            InitializeComponent();
            //dataGrid.ItemsSource = PersistentTracker.Instance.jobs;
            
        }      

        public PersistentTracker Tracker => PersistentTracker.Instance;

        public bool IsDirty 
        {
            get
            {
                return PersistentTracker.Instance.Jobs.Any(job => job.HasChanged);
            }
        }

        private void dataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            dataView.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Descending));
        }

        public void OnOpened(object sender, EventArgs e)
        {
            dataGrid.Focus();
        }

        public void OnClosing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
