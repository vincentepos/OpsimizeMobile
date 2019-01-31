using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using OM.Data;
using OM.Droid.Data;

[assembly: Xamarin.Forms.Dependency(typeof(NetworkConnection))]

namespace OM.Droid.Data
{
    public class NetworkConnection : INetworkConnection
    {
        public bool IsConnceted { get; set; }

        public void CheckInternetConnection()
        {
            var ConnectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var ActiveNetworkInfo = ConnectivityManager.ActiveNetworkInfo;
            if(ActiveNetworkInfo != null && ActiveNetworkInfo.IsConnectedOrConnecting)
            {
                IsConnceted = true;
            }
            else
            {
                IsConnceted = false;
            }
        }
    }
}