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
	public partial class OrderSizeSelect : ContentPage
	{
        public ObservableCollection<OrderSizeList> YourCollection { get; set; }
        public ProductLine PL { get; set; }
        public List<ProductLine> Items = new List<ProductLine>();
        public string username;
        public string password;
        public int PID;

        public OrderSizeSelect (string un, string pw, ProductLine LineItem, List<ProductLine> Products)
		{
            username = un;
            password = pw;
            PID = LineItem.ProductID;
            Items = Products;
			InitializeComponent ();
            Init();
            GetOrderSizes(LineItem);
            BindingContext = new OrderSizeList();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            DesciptionText.TextColor = Constants.MainTextColor;
            CodeText.TextColor = Constants.MainTextColor;
            QtyText.TextColor = Constants.MainTextColor;
            SupplierText.TextColor = Constants.MainTextColor;
        }

        async void GetOrderSizes(ProductLine OS)
        {
            PL = OS;
            Items.Remove(PL);
            CodeText.Text = OS.Code;
            DesciptionText.Text = OS.Name;
            QtyText.Text = OS.Qty.ToString();
            SupplierText.Text = OS.Supplier;

            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.GetOrderSizesUrl + "?ProductID=" + OS.ProductID);

            var osJson = await response.Content.ReadAsStringAsync();
            OrderSize ObjOSList = new OrderSize();
            if (osJson != "")
            {
                ObjOSList = JsonConvert.DeserializeObject<OrderSize>(osJson);
            }
            OrderSizePicker.ItemsSource = ObjOSList.OrderSizeList;
            OrderSizePicker.SelectedIndex = 1;
        }

        async void UpdateOSProcedure(object sender, EventArgs e)
        {
            PL.OrderSize = OSText.Text;
            PL.OrderSizeID = Convert.ToInt64(OSIDext.Text);
            Console.WriteLine("Order Ref: " + PL.OrderRef);
            Console.WriteLine(PL.OrderSize + " Selected as order size & " + PL.OrderSizeID + " as Order Size ID" );
            Items.Add(new ProductLine { Code = PL.Code, Name = PL.Name, Supplier = PL.Supplier, ProductID = PL.ProductID, OrderSize = PL.OrderSize, OrderSizeID = PL.OrderSizeID, ProQty = PL.ProQty });
            

            if (OrderSizePicker.SelectedItem != null)
            {
                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

                var jstring = JsonConvert.SerializeObject(PL);
                var content = new StringContent(jstring, Encoding.UTF8, "application/json");
                var response = client.PostAsync(Constants.UpdateOSUrl, content).Result;
                var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);

                if (result.Response != false)
                {
                    App.Current.MainPage = new EditProducts(username, password, PL.OrderRef, result.Status, result.Location, result.User, result.DateOrdered, Items, result.SiteID);
                }
                else
                {
                    await DisplayAlert("Upload Error", result.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Alert", "Please Select a Site.", "Ok");
            }
        }
    }
}