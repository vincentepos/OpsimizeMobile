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
	public partial class OpenPOList : ContentPage
	{
        public List<Item> Items = new List<Item>();
        public string username;
        public string password;

        public OpenPOList (string un, string pw)
		{
            username = un;
            password = pw;
			InitializeComponent ();
            GetPO();
            Init();
            InitSearchBar();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            po_search.BackgroundColor = Constants.BackgroundColor;
        }

        public async void GetPO()
        {
            //string user = "vince";
            //string password = "Qwe123";
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.PoUrl);

            var poJson = await response.Content.ReadAsStringAsync();
            PurchaseOrders ObjPOList = new PurchaseOrders();
            if(poJson != "")
            {
                ObjPOList = JsonConvert.DeserializeObject<PurchaseOrders>(poJson);
            }
            Items = ObjPOList.Items;
            var sorted = Items.OrderByDescending(x => x.DateOrdered).ToList();
            PurchaseOrdersListView.ItemsSource = sorted;
            PurchaseOrdersListView.ItemTapped += PurchaseOrdersListView_ItemTapped;
            
            //var po = JsonConvert.DeserializeObject<Dictionary<object, List<PurchaseOrders>>>(response);
            //Console.WriteLine("Log List" + po);
            //PurchaseOrdersListView.ItemsSource = po;
            
        }

        public void PurchaseOrdersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Item item = (Item)e.Item;

            if(item.Status == "Draft PO")
            {
                Navigation.PushAsync(new EditPO(username, password, item.OrderRef, item.Status,item.Location, item.User, item.DateOrdered, item.SiteID, item.OrderValue));
            }
            else
            {
                Navigation.PushAsync(new POView(item, username, password));
            }
            
        }

        void InitSearchBar()
        {
            po_search.TextChanged += (s, e) => FilterItem(po_search.Text);
            po_search.SearchButtonPressed += (s, e) => FilterItem(po_search.Text);
        }

        private void FilterItem(string filter)
        {
            PurchaseOrdersListView.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                PurchaseOrdersListView.ItemsSource = Items;
            }
            else
            {
                PurchaseOrdersListView.ItemsSource = Items.Where(X => (X.OrderRef.ToLower().Contains(filter.ToLower()) || X.Supplier.ToLower().Contains(filter.ToLower()) || X.Status.ToLower().Contains(filter.ToLower())));
            }
            PurchaseOrdersListView.EndRefresh();
        }
    }
}