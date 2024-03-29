﻿using System;
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

        public static string NoInternetText = "No Internet Connection";

        //-------------Base URL------------//
        public static string BaseURL = "https://app.opsimize.com/rest/mobileapp/v1/";
        //public static string BaseURL = "http://opsimize.eposability.com/rest/mobileapp/v1/";
        //public static string BaseURL = "http://10.0.2.2:8081/rest/mobileapp/v1/";

        //-------------Login------------//
        public static string LoginUrl = BaseURL + "Token";

        //-------------PO List---------------//
        public static string PoUrl = BaseURL + "PurchaceOrderList";

        //-------------PO Ref---------------//
        public static string PoRefUrl = BaseURL + "PurchaceOrder"; // ?PORef=14114282

        //-------------PO Line List---------------//
        public static string PoLineUrl = BaseURL + "PurchaceOrderLines"; // ?PORef=1411428 

        //-------------Take Delivery---------------//
        public static string TakeDeliveryUrl = BaseURL + "TakeDelivery"; // ?OrderRef=1411428

        //-------------DN Line List---------------//
        public static string DNLineUrl = BaseURL + "TakeDelivery/GetLines"; // ?PORef=1411428

        //-------------Post Delivery---------------//
        public static string PostDeliveryUrl = BaseURL + "TakeDelivery/Delivery";

        //-------------PO Site List---------------//
        public static string POSitesUrl = BaseURL + "PurchaceOrder/Sites";

        //-------------PO Create---------------//
        public static string POCreateUrl = BaseURL + "PurchaceOrder/Create";

        //-------------PO Site Update---------------//
        public static string POSiteUpdateUrl = BaseURL + "PurchaceOrder/POUpdate";

        //-------------PO Products---------------//
        public static string POProductsUrl = BaseURL + "PurchaceOrder/POProducts";

        //-------------PO Lines Post---------------//
        public static string POLinesPostUrl = BaseURL + "PurchaceOrder/POLines";

        //-------------PO Lines Post with Send---------------//
        public static string POLinesPostSendUrl = BaseURL + "PurchaceOrder/POLinesSend";

        //-------------Save PO---------------//
        public static string SendPOUrl = BaseURL + "PurchaceOrder/SendPO";

        //-------------Send PO---------------//
        public static string SavePOUrl = BaseURL + "PurchaceOrder/SavePO";

        //-------------Get OrderSizes---------------//
        public static string GetOrderSizesUrl = BaseURL + "PurchaceOrder/OrderSizes";

        //-------------Get POLine From ID---------------//
        public static string POLineFromIDUrl = BaseURL + "PurchaceOrder/POLineFromID"; // ?POLineRef=254

        //-------------Delete PO---------------//
        public static string DeletePOUrl = BaseURL + "PurchaceOrder/DeletePO"; // ?PORef=1411428 

        //-------------Order Size Update---------------//
        public static string UpdateOSUrl = BaseURL + "PurchaceOrder/OrderSizeUpdate";

        //-------------New Supplier Product---------------//
        public static string NewSPProductUrl = BaseURL + "Products/NewProduct";

        //-------------Update Supplier Product---------------//
        public static string UpdateProductS1Url = BaseURL + "Products/UpdateProductStep1";

        //-------------Device Token------------//
        public static string DeviceToken = BaseURL + "Token/DeviceToken"; // ?Token=123456

        //-------------Device Token IOS------------//
        public static string DeviceTokenIOS = BaseURL + "Token/DeviceTokenIOS"; // ?Token=123456

        //-------------Get Last Notification---------------//
        public static string LastNotification = BaseURL + "Notifications/LastNotification";

        //-------------Get All Notification---------------//
        public static string AllNotifications = BaseURL + "Notifications/AllNotifications ";

        //-------------Get Last Notification---------------//
        public static string Cashup = BaseURL + "Cashup/TradingPeriodByID"; //?TradingDateID=6374

        //-------------Get Tills By Site---------------//
        public static string TillList = BaseURL + "Cashup/GetTillsBySite"; //?SiteID=40

        //-------------Get Site List---------------//
        public static string SiteList = BaseURL + "Sites/SiteList"; 
    }
}