using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    class POLines
    {
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrderSize { get; set; }
        public double Qty { get; set; }
        public double Net { get; set; }
        public double VAT { get; set; }
        public double Gross { get; set; }
    }
}
