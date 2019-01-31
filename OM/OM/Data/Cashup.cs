using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    public class CashupList
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
