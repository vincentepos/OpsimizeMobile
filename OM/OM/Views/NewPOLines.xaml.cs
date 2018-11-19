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
	public partial class NewPOLines : ContentPage
	{
        public List<ProductLine> Items = new List<ProductLine>();
        public List<ProductLine> PROItems = new List<ProductLine>();
        public string username;
        public string password;
        public string _OrderReference;
        public string _OrderStatus;
        public string _OrderFor;
        public string _OrderBy;
        public DateTime _OrderDeliveryDate;
        public string _Site;

        public NewPOLines (string un, string pw, string OrderRef, string Status, string OrderFor, string OrderBy, DateTime DeliveryDate)
		{
            username = un;
            password = pw;
            _OrderReference = OrderRef;
            _OrderStatus = Status;
            _OrderFor = OrderFor;
            _OrderBy = OrderBy;
            _OrderDeliveryDate = DeliveryDate;
            InitializeComponent ();
            Init();
            GetProduct();
            InitSearchBar();
            Console.WriteLine("Pass Items (PO Lines): " + OrderRef + " " + Status + " " + OrderFor + " " + OrderBy);
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Console.WriteLine("Pass Items (PO Lines After): " + _OrderReference + " " + _OrderStatus + " " + _OrderFor + " " + _OrderBy + " " + _Site);
        }

        public async void GetProduct()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.POProductsUrl);

            var productJson = await response.Content.ReadAsStringAsync();
            POProducts ObjProductList = new POProducts();
            if (productJson != "")
            {
                ObjProductList = JsonConvert.DeserializeObject<POProducts>(productJson);
            }
            POLineListView.ItemsSource = ObjProductList.Lines;
            Items = ObjProductList.Lines;
            //Switcher.Toggled += switcher_Toggled;
        }

        void InitSearchBar()
        {
            poline_search.TextChanged += (s, e) => FilterItem(poline_search.Text);
            poline_search.SearchButtonPressed += (s, e) => FilterItem(poline_search.Text);
        }

        private void FilterItem(string filter)
        {
            POLineListView.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                POLineListView.ItemsSource = Items;
            }
            else
            {
                POLineListView.ItemsSource = Items.Where(X => X.Name.ToLower().Contains(filter.ToLower()));
            }
            POLineListView.EndRefresh();
        }

        void OnItemTapped(Object sender, ItemTappedEventArgs e)
        {
            var dataItem = (ProductLine)(e.Item);
            Items.Add(new ProductLine { Code = dataItem.Code, Name = dataItem.Name, Supplier = dataItem.Supplier, ProductID = dataItem.ProductID });
            PROItems.Add(new ProductLine { Code = dataItem.Code, Name = dataItem.Name, Supplier = dataItem.Supplier, ProductID = dataItem.ProductID });
            Console.WriteLine("Item Added :" + PROItems);
            DisplayAlert("Item Added", "Product added to Purchase Order", "Ok");
        }

        async void AddProductsDoneProcedure(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPOView(username, password, _OrderReference, _OrderStatus, _OrderFor, _OrderBy, _OrderDeliveryDate, PROItems));
        }
    }
}