using Newtonsoft.Json;
using OM.Data;
using OM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public partial class SiteListView : ContentPage
	{
        public ObservableCollection<RootSite> YourCollection { get; set; }
        public List<RootSite> Items = new List<RootSite>();
        public string username;
        public string password;
        public DeviceToken deviceToken = new DeviceToken();
        public long SiteID;

        public SiteListView (User user)
		{
            username = user.Username;
            password = user.Password;
            InitializeComponent();
            App.StartCheckIfInternet(lbl_NoInternet, this);
            Init();
            GetSiteList();
            InitSearchBar();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            notification_search.BackgroundColor = Constants.BackgroundColor;
        }

        public async void GetSiteList()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.SiteList);

            var siteJson = await response.Content.ReadAsStringAsync();
            RootSiteList ObjList = new RootSiteList();
            if (siteJson != "")
            {
                ObjList = JsonConvert.DeserializeObject<RootSiteList>(siteJson);
            }
            YourCollection = new ObservableCollection<RootSite>(ObjList.SiteList);
            Items = ObjList.SiteList;
            var sorted = YourCollection.OrderBy(x => x.SiteName).ToList();
            SiteListView2.ItemsSource = sorted;
            SiteListView2.ItemTapped += SiteListView2_ItemTapped;
        }

        public void SiteListView2_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RootSite item = (RootSite)e.Item;
            SiteID = item.SiteID;
            Navigation.PushAsync(new CashupListView(username, password, SiteID));

        }

        void InitSearchBar()
        {
            notification_search.TextChanged += (s, e) => FilterItem(notification_search.Text);
            notification_search.SearchButtonPressed += (s, e) => FilterItem(notification_search.Text);
        }

        private void FilterItem(string filter)
        {
            SiteListView2.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                SiteListView2.ItemsSource = Items;
            }
            else
            {
                SiteListView2.ItemsSource = Items.Where(X => (X.SiteName.ToLower().Contains(filter.ToLower())));
            }
            SiteListView2.EndRefresh();
        }
    }
}