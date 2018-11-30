using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class OrderSize
    {
        public int ProductID { get; set; }
        public List<OrderSizeList> OrderSizeList { get; set; }
    }

    public class OrderSizeList : INotifyPropertyChanged
    {
        public string OrderSize { get; set; }
        public int ProductID { get; set; }

        Site selectedPID;
        public Site SelectedPID
        {
            get { return selectedPID; }
            set
            {
                if (selectedPID != value)
                {
                    selectedPID = value;
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
}
