using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System.ComponentModel;

namespace OM.Data
{
    public class CashupList : INotifyPropertyChanged
    {
        public double ExpectedTill { get; set; }
        public double ActualTillEpos { get; set; }
        public double ActualTillActual { get; set; }
        public double ActualTillVariance { get; set; }
        public string CashupBy { get; set; }
        public DateTime CashupDate { get; set; }
        public string CashupNotes { get; set; }
        public List<SalesT> SalesT { get; set; }
        public List<PayT> PayT { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ExpandHideData
        {
            get
            {
                return new Command(() =>
                {
                    if (ExpanderButtonText == "+")
                    {
                        ExpandedHeight = 150;
                        ExpanderButtonText = "-";
                        TextColor = "White";
                        SalVisible = true;
                    }
                    else
                    {
                        ExpandedHeight = 0;
                        ExpanderButtonText = "+";
                        TextColor = "Transparent";
                        SalVisible = false;
                    }
                });
            }
        }

        private int _ExpandingHeight = 0;
        public int ExpandedHeight
        {
            get
            {
                return _ExpandingHeight;
            }
            private set
            {
                _ExpandingHeight = value;
                OnPropertyChanged();
            }
        }

        private string _TextColor = "White";
        public string TextColor
        {
            get
            {
                return _TextColor;
            }
            private set
            {
                _TextColor = value;
                OnPropertyChanged();
            }
        }

        private bool _SalVisible = true;
        public bool SalVisible
        {
            get
            {
                return _SalVisible;
            }
            set
            {
                _SalVisible = value;
                OnPropertyChanged();
            }
        }

        protected string _ExpanderButtonText = "+";
        public string ExpanderButtonText
        {
            get
            {
                return _ExpanderButtonText;
            }
            private set
            {
                _ExpanderButtonText = value;
                OnPropertyChanged();
            }
        }

        public ICommand ExpandHideData1
        {
            get
            {
                return new Command(() =>
                {
                    if (ExpanderButtonText1 == "+")
                    {
                        ExpandedHeight1 = 100;
                        ExpanderButtonText1 = "-";
                        TextColor1 = "White";
                        PayVisible = true;
                    }
                    else
                    {
                        ExpandedHeight1 = 0;
                        ExpanderButtonText1 = "+";
                        TextColor1 = "Transparent";
                        PayVisible = false;
                    }
                });
            }
        }

        private int _ExpandingHeight1 = 0;
        public int ExpandedHeight1
        {
            get
            {
                return _ExpandingHeight1;
            }
            private set
            {
                _ExpandingHeight1 = value;
                OnPropertyChanged();
            }
        }

        private string _TextColor1 = "White";
        public string TextColor1
        {
            get
            {
                return _TextColor1;
            }
            private set
            {
                _TextColor1 = value;
                OnPropertyChanged();
            }
        }

        private bool _PayVisible = true;
        public bool PayVisible
        {
            get
            {
                return _PayVisible;
            }
            set
            {
                _PayVisible = value;
                OnPropertyChanged();
            }
        }

        protected string _ExpanderButtonText1 = "+";
        public string ExpanderButtonText1
        {
            get
            {
                return _ExpanderButtonText1;
            }
            private set
            {
                _ExpanderButtonText1 = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class SalesT
    {
        public string Name { get; set; }
        public double Net { get; set; }
        public double Vat { get; set; }
        public double Gross { get; set; }
    }

    public class PayT
    {
        public string Name { get; set; }
        public double Epos { get; set; }
        public double Actual { get; set; }
        public double Variance { get; set; }
    }
}
