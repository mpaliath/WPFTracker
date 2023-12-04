using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WPFTracker.Controls
{
    /// <summary>
    /// Interaction logic for PopupControl.xaml
    /// </summary>
    public partial class InputPopupControl: UserControl
    {
       

        public InputPopupControl()
        {
            InitializeComponent();
           
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
            (PopupContent.Content as IAcceptInput).OnOpened(sender, e);
        }

        public void OpenPopup(IAcceptInput popup)
        {
            PopupContent.Content = popup;
            InputPopup.IsOpen = true;
          //  Grid.SetRowSpan(InputPopup, 2);
        }

        public event EventHandler<EventArgs>? OnPopupClosed;

        // Define a delegate for the event
        public delegate bool OnPopupClosingDelegate();

        // Define the event using the delegate
        public event OnPopupClosingDelegate OnPopupClosing;

        private bool CanClosePopup()
        {
            if(OnPopupClosing != null)
            {
                foreach(OnPopupClosingDelegate subscriber in OnPopupClosing.GetInvocationList())
                {
                    if (!subscriber.Invoke())
                        return false;
                }

                
            }

            return true;
        }

        private void InnerControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if(!(PopupContent.Content as IAcceptInput).IsDirty)
                    ClosePopup(null, null);
            }

            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (!(PopupContent.Content as IAcceptInput).IsDirty || CanClosePopup())
                    ClosePopup(null, null);
            }
        }
    }
}
