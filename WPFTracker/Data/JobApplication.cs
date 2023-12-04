using System;
using System.ComponentModel;

namespace WPFTracker.Data
{
    public class JobApplication : IModifiable
    {
        public JobApplication(DateTime date, string company, string appLink, string designation) 
        { 
            this.date = date;
            this.originalDate = date;

            this.company = company;
            this.originalCompany = company;


            this.appLink = appLink;
            this.originalAppLink = appLink;

            this.designation = designation;
            this.originalDesignation = designation;
        }

        private DateTime date;
        private DateTime originalDate;

        public DateTime Date 
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    HasChanged = true;
                }
            }
        }

        private string company;
        private string originalCompany;

        public string Company 
        {
            get { return company; }
            set
            {
                if (company != value)
                {
                    company = value;
                    HasChanged = true;
                }
            }
        }

        private string appLink;
        private string originalAppLink;

        public string AppLink
        {
            get { return appLink; }
            set
            {
                if (appLink != value)
                {
                    appLink = value;
                    HasChanged = true;
                }
            }
        }

        private string designation;
        private string originalDesignation;

        public string Designation
        {
            get { return designation; }
            set
            {
                if (designation != value)
                {
                    designation = value;
                    HasChanged = true;
                }
            }
        }


        private bool _hasChanged;

        public bool HasChanged
        {
            get { return _hasChanged; }
            set
            {
                if(value == true)
                {
                    if (originalAppLink == appLink && originalDesignation == designation && originalCompany == company && originalDate == date)
                        value = false;
                }
                if (_hasChanged != value)
                {
                    _hasChanged = value;
                    OnPropertyChanged(nameof(HasChanged));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
