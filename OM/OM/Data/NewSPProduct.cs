using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OM.Data
{
    public class NewSPProduct
    {
        public int ProductID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int SupplierID { get; set; }
        public int UOMID { get; set; }
        public List<OUMList> OUMList { get; set; }
        public List<SupplierList> SupplierList { get; set; }
    }

    public class OUMList : INotifyPropertyChanged
    {
        public string UOMName { get; set; }
        public int UOMID { get; set; }

        OUMList selectedUOM;
        public OUMList SelUOM
        {
            get { return selectedUOM; }
            set
            {
                if (selectedUOM != value)
                {
                    selectedUOM = value;
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

    public class SupplierList : INotifyPropertyChanged
    {
        public int SupplierID { get; set; }
        public string SupplierName { get; set; }

        SupplierList selectedSUP;
        public SupplierList SelSUP
        {
            get { return selectedSUP; }
            set
            {
                if (selectedSUP != value)
                {
                    selectedSUP = value;
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
