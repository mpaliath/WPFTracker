using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace WPFTracker
{
    class PersistentTracker : INotifyPropertyChanged
    {
        private int filedTodayCount;
        private int filedAppsCount;

        public int FiledTodayCount
        {
            get { return filedTodayCount; }
            set
            {
                if (filedTodayCount != value)
                {
                    filedTodayCount = value;
                    OnPropertyChanged(nameof(FiledTodayCount));
                }
            }
        }

        public int FiledAppsCount
        {
            get { return filedAppsCount; }
            set
            {
                if (filedAppsCount != value)
                {
                    filedAppsCount = value;
                    OnPropertyChanged(nameof(FiledAppsCount));
                }
            }
        }

        private PersistentTracker()
        {
            filedTodayCount = 0;
            filedAppsCount = 0;
        }

        public static PersistentTracker Instance = new PersistentTracker();

        public (int totalApps, int appsToday) CountApps()
        {
            int totalApps = 0;
            int appsToday = 0;

            var todayMarker = DateTime.Now.ToString("d");

            var skippedHeader = false;

            using (StreamReader reader = new StreamReader(TrackingFilePath))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!skippedHeader)
                    {
                        skippedHeader = true;
                        continue;
                    }
                    totalApps++;

                    if (line.StartsWith(todayMarker))
                        appsToday++;
                }
            }

            this.FiledTodayCount = appsToday;
            this.FiledAppsCount = totalApps;

            return (totalApps, appsToday);
        }

        public void TrackApp(string companyName, string appLink)
        {
            var newLine = DateTime.Now.ToString("d") + "," + companyName + "," + appLink;
            using StreamWriter writer = new StreamWriter(TrackingFilePath, true);
            writer.WriteLine(newLine);
        }

        private const string TrackingFilePath = "C:\\Users\\mahes\\source\\repos\\WPFTracker\\TrackedApps.csv";

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
