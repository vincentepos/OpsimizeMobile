using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class Site : INotifyPropertyChanged
    {
        public string PORef { get; set; }
        public string SiteName { get; set; }
        public long SiteID { get; set; }

        Site selectedSite;
        public Site SelectedSite
        {
            get { return selectedSite; }
            set
            {
                if (selectedSite != value)
                {
                    selectedSite = value;
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Sites
    {
        public string PORef { get; set; }
        public long SiteID { get; set; }
        public List<Site> AllSites { get; set; }
    }

    

    
}
