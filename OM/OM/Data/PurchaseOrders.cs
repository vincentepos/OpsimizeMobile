using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
   public class Item
    {
        private Item items;

        public Item(Item items)
        {
            this.items = items;
        }

        public DateTime DateOrdered { get; set; }
        public string OrderRef { get; set; }
        public string Location { get; set; }
        public double OrderValue { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string User { get; set; }
        public string Supplier { get; set; }
        public long SiteID { get; set; }
        public string SiteName { get; set; }
    }

    public class PurchaseOrders
    {
        public List<Item> Items { get; set; }
    }
}
