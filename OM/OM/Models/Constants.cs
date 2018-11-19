using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OM.Models
{
    public class Constants
    {
        public static bool isDev = true;

        public static Color BackgroundColor = Color.FromRgb(0,160,222);
        public static Color MainTextColor = Color.White;
        public static int TextLineHeight = 12;

        public static int LoginIconHeight = 120;

        ////-------------Login------------//
        //public static string LoginUrl = "https://app.opsimize.com/rest/mobileapp/v1/Token";

        ////-------------PO List---------------//
        //public static string PoUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrderList";

        ////-------------PO Ref---------------//
        //public static string PoRefUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrder"; // ?PORef=14114282

        ////-------------PO Line List---------------//
        //public static string PoLineUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrderLines"; // ?PORef=1411428

        ////-------------Take Delivery---------------//
        //public static string TakeDeliveryUrl = "https://app.opsimize.com/rest/mobileapp/v1/TakeDelivery"; // ?OrderRef=1411428

        ////-------------DN Line List---------------//
        //public static string DNLineUrl = "https://app.opsimize.com/rest/mobileapp/v1/TakeDelivery/GetLines"; // ?PORef=1411428

        ////-------------Post Delivery---------------//
        //public static string PostDeliveryUrl = "https://app.opsimize.com/rest/mobileapp/v1/TakeDelivery/Delivery";

        ////-------------PO Site List---------------//
        //public static string POSitesUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrder/Sites";

        ////-------------PO Create---------------//
        //public static string POCreateUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrder/Create";

        ////-------------PO Site Update---------------//
        //public static string POSiteUpdateUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrder/POUpdate";

        ////-------------PO Products---------------//
        //public static string POProductsUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrder/POProducts";

        ////-------------PO Lines Post---------------//
        //public static string POLinesPostUrl = "https://app.opsimize.com/rest/mobileapp/v1/PurchaceOrder/POLines";

        //-------------Login------------//
        public static string LoginUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/Token";

        //-------------PO List---------------//
        public static string PoUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrderList";

        //-------------PO Ref---------------//
        public static string PoRefUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrder"; // ?PORef=14114282

        //-------------PO Line List---------------//
        public static string PoLineUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrderLines"; // ?PORef=1411428

        //-------------Take Delivery---------------//
        public static string TakeDeliveryUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/TakeDelivery"; // ?OrderRef=1411428

        //-------------DN Line List---------------//
        public static string DNLineUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/TakeDelivery/GetLines"; // ?PORef=1411428

        //-------------Post Delivery---------------//
        public static string PostDeliveryUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/TakeDelivery/Delivery";

        //-------------PO SIte List---------------//
        public static string POSitesUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrder/Sites";

        //-------------PO Create---------------//
        public static string POCreateUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrder/Create";

        //-------------PO Site Update---------------//
        public static string POSiteUpdateUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrder/POUpdate";

        //-------------PO POProducts---------------//
        public static string POProductsUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrder/POProducts";

        //-------------PO Lines Post---------------//
        public static string POLinesPostUrl = "http://10.0.2.2:8081/rest/mobileapp/v1/PurchaceOrder/POLines";
    }
}
