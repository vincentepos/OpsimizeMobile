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
        public List<ProductLine> POProducts = new List<ProductLine>();
        public POProducts POProductsList = new POProducts();
        public string username;
        public string password;
        public string OrderRef;
        public string siteid;
        public string _OrderStatus;
        public string _OrderFor;
        public string _OrderBy;
        public DateTime _OrderDeliveryDate;
        public long _Site;

        public Site SiteUpdate = new Site();

        public NewPO (string un, string pw)
		{
            username = un;
            password = pw;
            InitializeComponent ();
            Init();
            GetSiteList();
            GetNewPO();
            BindingContext = new Site();
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
            SiteUpdate.PORef = po.OrderRef;
            POProductsList.OrderRef = po.OrderRef;
            OrderRef = po.OrderRef;
            _OrderBy = po.User;
            _OrderDeliveryDate = po.DeliveryDate;
            _OrderFor = po.OrderFor;
            _OrderStatus = po.Status;
            

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
            Console.WriteLine("Site: " + SiteIDText.Text);
            

        }

        async void AddProductsProcedure(object sender, EventArgs e)
        {
            if (SitePicker.SelectedItem != null)
            {
                Console.WriteLine("Site: " + SiteIDText.Text);
                siteid = SiteIDText.Text;
                SiteUpdate.SiteID = Convert.ToInt64(siteid);
                _Site = SiteUpdate.SiteID;
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

                var jstring = JsonConvert.SerializeObject(SiteUpdate);
                var content = new StringContent(jstring, Encoding.UTF8, "application/json");
                var response = client.PostAsync(Constants.POSiteUpdateUrl, content).Result;
                var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);

                Console.WriteLine("Pass Items: " + OrderRef + " " + _OrderStatus + " " + _OrderFor + " " + _OrderBy);
                await Navigation.PushAsync(new NewPOLines(username, password, OrderRef, _OrderStatus, _OrderFor, _OrderBy, _OrderDeliveryDate, _Site));
            }
            else
            {
                await DisplayAlert("Alert", "Please Select a Site.", "Ok");
            }
            
        }

        
    }
}