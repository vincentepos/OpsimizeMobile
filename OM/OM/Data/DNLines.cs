using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    public class DeliveryNoteLines
    {
        public List<DNLine> DNLines { get; set; }
    }

    public class DNLine
    {
        public double QtyReceived { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrderSize { get; set; }
        public double Qty { get; set; }
        public double Net { get; set; }
        public double VAT { get; set; }
        public double Gross { get; set; }
        public int ID { get; set; }
    }
}
