using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class POProducts
    {
        public string OrderRef { get; set; }
        public long SiteID { get; set; }
        public List<ProductLine> Lines { get; set; }
    }

    public class ProductLine : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Supplier { get; set; }
        public int ProductID { get; set; }
        public string Code { get; set; }
        public double Qty { get; set; }
        public string OrderSize { get; set; }

        public double ProQty
        {
            get { return Qty; }
            set
            {
                Qty = value;
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
