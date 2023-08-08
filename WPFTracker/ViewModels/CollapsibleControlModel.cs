using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace WPFTracker.ViewModels
{
    public class CollapsibleControlModel : INotifyPropertyChanged, ICanBeCollapsed
    {
        public CollapsibleControlModel(GridLength defaultLength)
        {
            collapsibleColumnWidth = defaultLength;
        }

        public void Collapse()
        {
            CollapsibleColumnWidth = new GridLength(0);
        }

        public void Expand()
        {
            CollapsibleColumnWidth = new GridLength(1, GridUnitType.Star);
        }


        private GridLength collapsibleColumnWidth;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public GridLength CollapsibleColumnWidth { get => collapsibleColumnWidth; set { collapsibleColumnWidth = value; OnPropertyChanged("CollapsibleColumnWidth"); } }
    }
}
