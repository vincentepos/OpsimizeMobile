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

namespace OM.Views.NewProducts
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewProduct_Step1 : ContentPage
	{
        public ObservableCollection<OUMList> UOMCollection { get; set; }
        public ObservableCollection<SupplierList> SUPCollection { get; set; }
        public List<ProductLine> Items = new List<ProductLine>();
        public string username;
        public string password;
        public string _OrderReference;
        public string _OrderStatus;
        public string _OrderFor;
        public string _OrderBy;
        public DateTime _OrderDeliveryDate;
        public string qty;
        public long _Site;
        public int _PROID;

        public NewProduct_Step1 (string un, string pw, string OrderRef, string Status, string OrderFor, string OrderBy, DateTime DeliveryDate, List<ProductLine> Products, long SiteID)
		{
            username = un;
            password = pw;
            Items = Products;
            _OrderReference = OrderRef;
            _OrderStatus = Status;
            _OrderFor = OrderFor;
            _OrderBy = OrderBy;
            _OrderDeliveryDate = DeliveryDate;
            _Site = SiteID;
            InitializeComponent ();
            Init();
            GetProduct();
            this.BindingContext = this;
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
        }

        async void GetProduct()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.NewSPProductUrl);
            Console.WriteLine("Product: " + response);
            var proJson = await response.Content.ReadAsStringAsync();
            NewSPProduct sppro = new NewSPProduct();
            if (proJson != "")
            {
                sppro = JsonConvert.DeserializeObject<NewSPProduct>(proJson);
            }
            _PROID = sppro.ProductID;
            UOMCollection = new ObservableCollection<OUMList>(sppro.OUMList);
            SUPCollection = new ObservableCollection<SupplierList>(sppro.SupplierList);
            SupplierPicker.ItemsSource = SUPCollection;
            UOMPicker.ItemsSource = UOMCollection;
        }

        async void Step2Procedure(object sender, EventArgs e)
        {
            await DisplayAlert("Not Enabled", "Not Enabled", "Ok");
            var uom = (OUMList)UOMPicker.SelectedItem;
            var sup = (SupplierList)SupplierPicker.SelectedItem;
            Console.WriteLine("Selected Supplier: " + sup.SupplierName);
            Console.WriteLine("Selected UOM: " + uom.UOMName);

            NewSPProduct P1 = new NewSPProduct
            {
                Code = CodeEntry.Text,
                Name = DescriptionEntry.Text,
                ProductID = _PROID,
                SupplierID = sup.SupplierID,
                UOMID = uom.UOMID
            };

            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

            var jstring = JsonConvert.SerializeObject(P1);
            var content = new StringContent(jstring, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Constants.UpdateProductS1Url, content).Result;
            var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);
            if (result != null)
            {
                //await Navigation.PushAsync(new EditPOLines(username, password, _OrderReference, _OrderStatus, _OrderFor, _OrderBy, _OrderDeliveryDate, _Site, PROItems));
            }
            else
            {
                await DisplayAlert("Error", "Can not go to next step, connection error.", "Ok");
            }
        }
    }
}