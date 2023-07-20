using System;
using System.Windows;
using System.Windows.Threading;
using WPFTracker.Properties;
using WPFTracker.Utilities;

namespace WPFTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Create the settings object
        internal static Settings AppSettings = Settings.Default;

        public App()
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            if (AppSettings.ShouldSwitchMode)
            {
                StartupUri = new Uri("Windows/Timer/TimedActivityWindow.xaml", UriKind.Relative);
            }
            else
            {
                StartupUri = new Uri("Windows/Tracker/TrackerWindow.xaml", UriKind.Relative);
            }
            base.OnStartup(e);
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Instance.LogException(e.Exception);
            e.Handled = true;
        }
    }
}
