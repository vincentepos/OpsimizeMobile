using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OM.Data;
using OM.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CashupListView : ContentPage
	{
        public ObservableCollection<Till> TillCollection { get; set; }
        public List<Till> Items = new List<Till>();
        public string username;
        public string password;
        public long SiteID;
        public long TID;

        public CashupListView (string un, string pw, long sid)
		{
            username = un;
            password = pw;
            SiteID = sid;
            InitializeComponent();
            Init();
            GetTillList();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            //notification_search.BackgroundColor = Constants.BackgroundColor;
        }

        public async void GetTillList()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.TillList + "?SiteID=" + SiteID);

            var tillJson = await response.Content.ReadAsStringAsync();
            TillList ObjList = new TillList();
            if (tillJson != "")
            {
                ObjList = JsonConvert.DeserializeObject<TillList>(tillJson);
            };
            TillCollection = new ObservableCollection<Till>(ObjList.Tills);
            var salessorted = TillCollection.OrderByDescending(x => x.CashupDate).ToList();
            TillListView.ItemsSource = salessorted;
            Items = salessorted;
            TillListView.ItemTapped += TillListView_ItemTapped;
        }

        public void TillListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Till item = (Till)e.Item;
            TID = item.TradingDateID;
            Navigation.PushAsync(new Cashup(username, password, TID));

        }

        //void InitSearchBar()
        //{
        //    notification_search.TextChanged += (s, e) => FilterItem(notification_search.Text);
        //    notification_search.SearchButtonPressed += (s, e) => FilterItem(notification_search.Text);
        //}

        //private void FilterItem(string filter)
        //{
        //    TillListView.BeginRefresh();
        //    if (string.IsNullOrWhiteSpace(filter))
        //    {
        //        TillListView.ItemsSource = Items;
        //    }
        //    else
        //    {
        //        TillListView.ItemsSource = Items.Where(X => (X.TillName.ToLower().Contains(filter.ToLower())));
        //    }
        //    TillListView.EndRefresh();
        //}
    }
}