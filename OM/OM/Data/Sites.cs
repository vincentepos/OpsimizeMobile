using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class Site : INotifyPropertyChanged
    {
        public string SiteName { get; set; }

        public string SiteSel
        {
            get { return SiteName; }
            set
            {
                SiteName = value;
                OnPropertyChanged();
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
        public List<Site> AllSites { get; set; }
    }

    

    
}
