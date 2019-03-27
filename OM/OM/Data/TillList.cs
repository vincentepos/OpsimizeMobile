using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class TillList
    {
        public int SiteID { get; set; }
        public List<Till> Tills { get; set; }
    }

    public class Till : INotifyPropertyChanged
    {
        public DateTime CashupDate { get; set; }
        public string TillName { get; set; }
        public decimal TillTotal { get; set; }
        public string TillStatus { get; set; }
        public string TillColor { get; set; }
        public int TradingDateID { get; set; }
        public bool Locked { get; set; }

        public string TillLocked
        {
            get
            {
                if(Locked == true)
                {
                    return "locked.png";
                }
                return "unlocked.png";
            }
            set
            {
                TillLocked = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
