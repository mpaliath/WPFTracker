using System.Windows;
using System.Windows.Threading;
using WPFTracker.Utilities;

namespace WPFTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;


        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Instance.LogException(e.Exception);
            e.Handled = true;
        }
    }
}
