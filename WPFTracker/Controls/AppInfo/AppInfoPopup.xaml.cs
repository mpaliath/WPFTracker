using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for AppInfoPopup.xaml
    /// </summary>
    public partial class AppInfoPopup : UserControl
    {
        public AppInfoPopup()
        {
            InitializeComponent();
            InnerControl.OnSubmit += ClosePopup;
        }

        private void ClosePopup(object? sender, EventArgs? e)
        {
            InputPopup.IsOpen = false;

            if (e != null)
            {
                OnPopupClosed?.Invoke(this, e);
            }
        }

        private void InputPopup_Opened(object sender, EventArgs e)
        {
            InnerControl.OnOpened(sender, e);
        }

        public void OpenPopup()
        {
            InputPopup.IsOpen = true;
        }

        public event EventHandler<EventArgs>? OnPopupClosed;

        private void InnerControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape
                && !InnerControl.IsDirty)
            {
                ClosePopup(null, null);
            }
        }
    }
}
