using Newtonsoft.Json;
using OM.Data;
using OM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class POView : ContentPage
	{
        public string poref;
        public string username;
        public string password;

        public POView (Item item, string un, string pw)
		{
            username = un;
            password = pw;
            InitializeComponent ();
            Init();
            PopulatePOView(item);
            GetPOLines(item);
            poref = item.OrderRef;
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            DateOrderedText.TextColor = Constants.MainTextColor;
            OrderRefText.TextColor = Constants.MainTextColor;
            LocationText.TextColor = Constants.MainTextColor;
            OrderValueText.TextColor = Constants.MainTextColor;
            StatusText.TextColor = Constants.MainTextColor;
            UserText.TextColor = Constants.MainTextColor;
        }

        void PopulatePOView(Item item)
        {
            DateOrderedText.Text = item.DateOrdered.ToString();
            OrderRefText.Text = item.OrderRef;
            LocationText.Text = item.Location;
            OrderValueText.Text = item.OrderValue.ToString();
            StatusText.Text = item.Status;
            UserText.Text = item.User;
        }

        public async void GetPOLines(Item item)
        {
            //string user = "vince";
            //string password = "Qwe123";
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.PoLineUrl + "?PORef=" + item.OrderRef);

            var poJson = await response.Content.ReadAsStringAsync();
            POLines ObjPOList = new POLines();
            if (poJson != "")
            {
                ObjPOList = JsonConvert.DeserializeObject<POLines>(poJson);
            }
            POLinesListView.ItemsSource = ObjPOList.Lines;
            //POLinesListView.ItemTapped += PurchaseOrdersListView_ItemTapped;
            GC.Collect();
        }

        void TakeDeliveryProcess(object sender, EventArgs e)
        {
            Console.WriteLine("Item Order Ref: "+poref);
            Navigation.PushModalAsync(new TakeDelivery(poref, username, password));
        }
    }
}