using System;
using System.Collections.Generic;
using System.Text;

namespace OM.Data
{
    public class RootSiteList
    {
        public List<RootSite> SiteList { get; set; }
    }

    public class RootSite
    {
        public string SiteName { get; set; }
        public int SiteID { get; set; }
    }
}
