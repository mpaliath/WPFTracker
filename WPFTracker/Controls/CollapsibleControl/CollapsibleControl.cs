using System.Windows.Controls;
using WPFTracker.ViewModels;

namespace WPFTracker.Controls
{
    public abstract class CollapsibleControl : UserControl
    {
        private readonly CollapsibleControlModel viewModel;

        protected CollapsibleControl(CollapsibleControlModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
        }

        private bool isCollapsed;

        public void ToggleSize()
        {
            if (isCollapsed)
            {
                this.viewModel.Expand();
                isCollapsed = false;
            }
            else
            {
                this.viewModel.Collapse();
                isCollapsed = true;
            }
        }
    }
}
