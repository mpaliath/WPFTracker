using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Markup;
using WPFTracker.Data;

namespace WPFTracker
{
    public class PersistentTracker : INotifyPropertyChanged
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

        private int contactsToday;
        private int totalContacts;

        public int ContactsToday
        {
            get { return contactsToday; }
            set
            {
                if (contactsToday != value)
                {
                    contactsToday = value;
                    OnPropertyChanged(nameof(ContactsToday));
                }
            }
        }
        public int TotalContacts
        {
            get { return totalContacts; }
            set
            {
                if (totalContacts != value)
                {
                    totalContacts = value;
                    OnPropertyChanged(nameof(TotalContacts));
                }
            }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private PersistentTracker()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Vendors = new ObservableCollection<VendorDetails>();

            Jobs = new ObservableCollection<JobApplication>();

            // Capture the current SynchronizationContext
#pragma warning disable CS8601 // Possible null reference assignment.
            _syncContext = SynchronizationContext.Current;
#pragma warning restore CS8601 // Possible null reference assignment.

            RefreshTracker();
        }

        private SynchronizationContext _syncContext;

        public void RefreshTracker()
        {
            // Use the SynchronizationContext to marshal the call to Vendors.Clear() to the main UI thread
            _syncContext.Post(_ =>
            {
                Vendors.Clear();
                Jobs.Clear();

                CacheTrackingInfo();
                UpdateProps();
            }, null);
        }

        public static PersistentTracker Instance = new PersistentTracker();
        private string[] TrackedAppsHeaders = new string[] { "Date", "Company", "AppLink/VendorName", "Type", "Designation" };

        public ObservableCollection<JobApplication> Jobs { get; }
        public ObservableCollection<VendorDetails> Vendors { get; }

        private void CacheTrackingInfo()
        {
            var skippedHeader = false;
            using (TextFieldParser parser = new TextFieldParser(TrackingFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                while (!parser.EndOfData)
                {

                    string[] fields = parser.ReadFields();
                    if (!skippedHeader)
                    {
                        skippedHeader = true;
                        continue;
                    }

                    var trackedItem = TrackedItemType.App;

                    if (fields.Length > 3)
                    {
                        if (fields[3] == TrackedItemType.Vendor.ToString())
                        {
                            trackedItem = TrackedItemType.Vendor;
                        }
                    }

                    if (trackedItem == TrackedItemType.App)
                    {

                        // Assuming the CSV columns are: Name, Age, Email
                        JobApplication job = new JobApplication(DateTime.Parse(fields[0]), fields[1], fields[2], "Unknown");

                        Jobs.Add(job);
                    }

                    else
                    {
                        VendorDetails vendor = new VendorDetails
                        {
                            Date = DateTime.Parse(fields[0]),
                            Company = fields[1],
                            Name = fields[2]

                        };

                        Vendors.Add(vendor);
                    }
                }
            }
        }

        public void UpdateProps()
        {
            int totalApps = Jobs.Count;
            int appsToday = 0;

            int totalVendors = Vendors.Count;
            int contactsToday = 0;

            foreach (var item in Jobs)
            {
                if (item.Date == DateTime.Today)
                {
                    appsToday++;
                }
            }

            foreach (var item in Vendors)
            {
                if (item.Date == DateTime.Today)
                    contactsToday++;
            }



            this.FiledTodayCount = appsToday;
            this.FiledAppsCount = totalApps;
            this.ContactsToday = contactsToday;
            this.TotalContacts = totalVendors;


        }

        public void UpdateApps()
        {
            using StreamWriter writer = new StreamWriter(TrackingFilePath, false);
            writer.WriteLine(string.Join(",", TrackedAppsHeaders.Select(item => "'" + item + "'")));
            foreach (var job in Jobs)
            {
                var newLine = job.Date + "," + job.Company + "," + job.AppLink + "," + TrackedItemType.App.ToString() + "," + job.Designation;
                writer.WriteLine(newLine);
                job.HasChanged = false;
            }
            foreach (var vendor in Vendors)
            {
                var newLine = vendor.Date + "," + vendor.Name + "," + vendor.Company + "," + TrackedItemType.Vendor.ToString();
                writer.WriteLine(newLine);
            }
        }

        public void TrackApp(string companyName, string appLink, string designation)
        {
            var itemtype = TrackedItemType.App.ToString();
            var newLine = DateTime.Now.ToString("d") + "," + companyName + "," + appLink + "," + itemtype +"," +designation;
            using StreamWriter writer = new StreamWriter(TrackingFilePath, true);
            writer.WriteLine(newLine);
            Jobs.Add(new JobApplication(DateTime.Today, companyName, appLink, designation));
            this.FiledTodayCount++;
            this.FiledAppsCount++;
        }

        public void TrackVendor(string name, string companyName)
        {
            var itemtype = TrackedItemType.Vendor.ToString();
            var newLine = DateTime.Now.ToString("d") + "," + name + "," + companyName + "," + itemtype;
            using StreamWriter writer = new StreamWriter(TrackingFilePath, true);
            writer.WriteLine(newLine);
            Vendors.Add(new VendorDetails() { Name = name, Company = companyName, Date = DateTime.Today });
            this.ContactsToday++;
            this.TotalContacts++;
        }

        public enum TrackedItemType { App, Vendor };

        public const string TrackingFilePath = "C:\\Users\\mahes\\source\\repos\\mpaliath\\WPFTracker\\TrackedApps.csv";
        public const string TrackingFilePath1 = "C:\\Users\\mahes\\source\\repos\\mpaliath\\WPFTracker\\TrackedApps1.csv";

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
