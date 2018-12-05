using System;
namespace OM.Data
{
    public class GeneralResponse
    {
        public bool Response { get; set; }
        public string Message { get; set; }
        public DateTime DateOrdered { get; set; }
        public string OrderRef { get; set; }
        public string Location { get; set; }
        public int OrderValue { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        public long SiteID { get; set; }
    }
}
