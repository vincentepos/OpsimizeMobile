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
	public partial class NewPOView : ContentPage
	{
        public POProducts POProductsList = new POProducts();
        public List<ProductLine> Items = new List<ProductLine>();
        public User user = new User();
        public string username;
        public string password;
        public string _OrderReference;
        public string _OrderStatus;
        public string _OrderFor;
        public string _OrderBy;
        public DateTime _OrderDeliveryDate;
        public string qty;
        public long _Site;

        public NewPOView (string un, string pw, string OrderRef, string Status, string OrderFor, string OrderBy, DateTime DeliveryDate, List<ProductLine> Products, long SiteID)
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
            user.Username = un;
            user.Password = pw;
            POProductsList.OrderRef = _OrderReference;
            Console.WriteLine("Root Order Ref: " + POProductsList.OrderRef);
            InitializeComponent ();
            Init();
            BindingContext = new ProductLine();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            DeliveryDateText.TextColor = Constants.MainTextColor;
            OrderRefText.TextColor = Constants.MainTextColor;
            OrderedByText.TextColor = Constants.MainTextColor;
            OrderForText.TextColor = Constants.MainTextColor;
            StatusText.TextColor = Constants.MainTextColor;

            POLineSumListView.ItemsSource = Items;

            DeliveryDateText.Text = _OrderDeliveryDate.ToString("dd/MM/yyyy HH:mm");
            OrderRefText.Text = _OrderReference;
            OrderedByText.Text = _OrderBy;
            OrderForText.Text = _OrderFor;
            StatusText.Text = _OrderStatus;
            

        }

        void QtyChange(object sender, TextChangedEventArgs e)
        {
            qty = e.NewTextValue;
            Console.WriteLine("value of: " + qty);
        }

        async void SaveProductsProcedure(object sender, EventArgs e)
        {
            POProductsList.Lines = Items;
            POProductsList.SiteID = _Site;
            Console.WriteLine("Root Order Ref End: " + POProductsList.OrderRef);
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

            var jstring = JsonConvert.SerializeObject(POProductsList);
            var content = new StringContent(jstring, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Constants.POLinesPostUrl, content).Result;
            var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);

            if (result.Response == true)
            {
                App.Current.MainPage = new NavigationPage(new Dashboard(user));
                await DisplayAlert("Successful", "Purchase Order(s) Successfully Created", "Ok");
            }
            else
            {
                await DisplayAlert("Not Uploaded", result.Message, "Ok");
            }
        }
    }
}