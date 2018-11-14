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
	public partial class NewPO : ContentPage
	{
        public List<Site> SiteList = new List<Site>();
        public string username;
        public string password;
        public string OrderRef;

        public NewPO (string un, string pw)
		{
            username = un;
            password = pw;
            InitializeComponent ();
            Init();
            GetSiteList();
            GetNewPO();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            DeliveryDateText.TextColor = Constants.MainTextColor;
            OrderRefText.TextColor = Constants.MainTextColor;
            OrderedByText.TextColor = Constants.MainTextColor;
            OrderForText.TextColor = Constants.MainTextColor;
            StatusText.TextColor = Constants.MainTextColor;
        }

        public async void GetNewPO()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.POCreateUrl);
            Console.WriteLine("PO Response: " + response);
            var poJson = await response.Content.ReadAsStringAsync();
            PO po = new PO();
            if (poJson != "")
            {
                po = JsonConvert.DeserializeObject<PO>(poJson);
            }
            OrderRefText.Text = po.OrderRef;
            DeliveryDateText.Text = po.DeliveryDate.ToString();
            OrderedByText.Text = po.User;
            StatusText.Text = po.Status;
            OrderForText.Text = po.OrderFor;
            //PO result = JsonConvert.DeserializeObject<PO>(response);
        }

        public async void GetSiteList()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.POSitesUrl);
            Console.WriteLine("Site Response: " + response);
            var siteJson = await response.Content.ReadAsStringAsync();
            Sites ObjSiteList = new Sites();
            if (siteJson != "")
            {
                ObjSiteList = JsonConvert.DeserializeObject<Sites>(siteJson);
            }
            SitePicker.ItemsSource = ObjSiteList.AllSites;
            SiteList = ObjSiteList.AllSites;
        }

        async void AddProductsProcedure(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Not implemented yet.", "Ok");
        }
    }
}