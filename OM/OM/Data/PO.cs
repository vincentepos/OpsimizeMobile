using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    public class PO
    {
        public DateTime DateOrdered { get; set; }
        public string OrderRef { get; set; }
        public string Location { get; set; }
        public string Supplier { get; set; }
        public double OrderValue { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string User { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderFor { get; set; }

        public PO() { }
        public PO(string OrderRef)
        {
            this.OrderRef = OrderRef;
        }

        public bool CheckPOInformation()
        {
            if (!this.OrderRef.Equals(""))
                return true;
            else
                return false;
        }


    }
}
