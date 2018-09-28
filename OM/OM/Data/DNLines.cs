using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class DeliveryNoteLines
    {
        public string PORef { get; set; }
        public string Base64 { get; set; }
        public List<DNLine> DNLines { get; set; }

        
    }

    public class DNLine : INotifyPropertyChanged
    {
        public double QtyReceived { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrderSize { get; set; }
        public string DNID { get; set; }
        public double Qty { get; set; }
        public double Net { get; set; }
        public double VAT { get; set; }
        public double Gross { get; set; }
        

        public double QtyRec
        {
            get { return QtyReceived; }
            set
            {
                QtyReceived = value;
                OnPropertyChanged();
            }
        }

        //public int _ID
        //{
        //    get { return ID; }
        //    set
        //    {
        //        ID = value;
        //        OnPropertyChanged();
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
