using OM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuickDelivery : ContentPage
	{
		public QuickDelivery ()
		{
			InitializeComponent ();
            Init();
            GetQuickDelivery();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_DNRef.TextColor = Constants.MainTextColor;
            Lbl_InvoiceRef.TextColor = Constants.MainTextColor;
            Lbl_OrderTotal.TextColor = Constants.MainTextColor;
            Lbl_POOrderRef.TextColor = Constants.MainTextColor;
        }

        public async void GetQuickDelivery()
        {
            string user = "vince";
            string password = "Qwe123";
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.PoUrl);
        }

    }
}