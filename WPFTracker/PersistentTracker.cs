using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
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

        private PersistentTracker()
        {
            Vendors = new ObservableCollection<VendorDetails>();

            Jobs = new ObservableCollection<JobApplication>();

            RefreshTracker();
        }

        public void RefreshTracker()
        {
            filedTodayCount = 0;
            filedAppsCount = 0;
            contactsToday = 0;
            totalContacts = 0;

            Vendors.Clear();
            Jobs.Clear();

            CacheTrackingInfo();
            UpdateProps();
        }

        public static PersistentTracker Instance = new PersistentTracker();

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
                        JobApplication job = new JobApplication
                        {
                            Date = DateTime.Parse(fields[0]),
                            Company = fields[1],
                            AppLink = fields[2]
                        };

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

        public void TrackApp(string companyName, string appLink)
        {
            var itemtype = TrackedItemType.App.ToString();
            var newLine = DateTime.Now.ToString("d") + "," + companyName + "," + appLink + "," + itemtype;
            using StreamWriter writer = new StreamWriter(TrackingFilePath, true);
            writer.WriteLine(newLine);
            Jobs.Add(new JobApplication() { Company = companyName, AppLink = appLink, Date = DateTime.Today });
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

        public const string TrackingFilePath = "C:\\Users\\mahes\\source\\repos\\WPFTracker\\TrackedApps.csv";

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
