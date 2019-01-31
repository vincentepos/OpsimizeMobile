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
	public partial class EditPO : ContentPage
	{
        public ObservableCollection<Line> YourCollection { get; set; }
        public string username;
        public string password;
        public string _OrderReference;
        public string _OrderStatus;
        public string _OrderFor;
        public string _OrderBy;
        public DateTime _OrderDeliveryDate;
        public double _OrderValue;
        public List<Line> LineList = new List<Line>();
        public List<ProductLine> PROItems = new List<ProductLine>();
        public long _Site;
        public Site SiteUpdate = new Site();
        public string qty;
        public User user = new User();

        public EditPO (string un, string pw, string OrderRef, string Status, string OrderFor, string OrderBy, DateTime DeliveryDate, long SiteID, double OrderValue)
		{
            username = un;
            password = pw;
            user.Username = un;
            user.Password = pw;
            _OrderReference = OrderRef;
            _OrderStatus = Status;
            _OrderFor = OrderFor;
            _OrderBy = OrderBy;
            _OrderDeliveryDate = DeliveryDate;
            _Site = SiteID;
            _OrderValue = OrderValue;
            SiteUpdate.PORef = OrderRef;
            SiteUpdate.SiteID = SiteID;
            InitializeComponent ();
            Init();
            EditPOView();
            BindingContext = new Line();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            DeliveryDateText.TextColor = Constants.MainTextColor;
            OrderRefText.TextColor = Constants.MainTextColor;
            OrderedByText.TextColor = Constants.MainTextColor;
            OrderForText.TextColor = Constants.MainTextColor;
            StatusText.TextColor = Constants.MainTextColor;
            OrderValueText.TextColor = Constants.MainTextColor;

            DeliveryDateText.Text = _OrderDeliveryDate.ToString("dd/MM/yyyy HH:mm");
            OrderRefText.Text = _OrderReference;
            OrderedByText.Text = _OrderBy;
            OrderForText.Text = _OrderFor;
            StatusText.Text = _OrderStatus;
            OrderValueText.Text = _OrderValue.ToString();
        }

        public async void EditPOView()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.PoLineUrl + "?PORef=" + _OrderReference);

            var poJson = await response.Content.ReadAsStringAsync();
            POLines ObjPOList = new POLines();
            if (poJson != "")
            {
                ObjPOList = JsonConvert.DeserializeObject<POLines>(poJson);
            }
            YourCollection = new ObservableCollection<Line>(ObjPOList.Lines);
            POLineSumListView.ItemsSource = YourCollection;
            LineList = ObjPOList.Lines;
            foreach (var item in LineList)
            {
                PROItems.Add(new ProductLine { Code = item.Code, Name = item.Description, Supplier = item.Supplier, ProductID = Convert.ToInt16(item.ProductID), OrderSize = item.OrderSize, Qty = item.Qty, OrderSizeID = item.OrderSizeID });
            }
        }

        async void AddProductsProcedure(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditPOLines(username, password, _OrderReference, _OrderStatus, _OrderFor, _OrderBy, _OrderDeliveryDate, _Site, PROItems));
        }

        void QtyChange(object sender, TextChangedEventArgs e)
        {
            qty = e.NewTextValue;
            Console.WriteLine("value of: " + qty);
        }

        async public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            Line dataItem = (Line)mi.CommandParameter;

            //----Call Delete PO Line API here ----// "?POLineRef="
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetStringAsync(Constants.POLineFromIDUrl + "?POLineRef=" + dataItem.POLineID);
            GeneralResponse result = JsonConvert.DeserializeObject<GeneralResponse>(response);
            if (result.Response == true)
            {
                YourCollection.Remove(dataItem);
                await DisplayAlert("Item Removed", dataItem.Description + " removed", "Ok");
            }
            else
            {
                await DisplayAlert("Item Not Removed", result.Message, "Ok");
            }

            
        }

        async void SavePOProcedure(object sender, EventArgs e)
        {
            if (LineList != null)
            {
                POLines PostPOLines = new POLines();
                PostPOLines.Lines = LineList;
                PostPOLines.SiteID = _Site;
                PostPOLines.OrderRef = _OrderReference;

                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

                var jstring = JsonConvert.SerializeObject(PostPOLines);
                var content = new StringContent(jstring, Encoding.UTF8, "application/json");
                var response = client.PostAsync(Constants.SavePOUrl, content).Result;
                var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);

                if (result.Response == true)
                {
                    await DisplayAlert("Successful", "Purchase Order Successfully Saved", "Ok");
                    App.Current.MainPage = new NavigationPage(new Dashboard(user));
                }
                else
                {
                    await DisplayAlert("Save Failed", result.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Alert", "Can not save as there are no Products", "Ok");
            }
        }

        async void SendPOProcedure(object sender, EventArgs e)
        {
            if (LineList != null)
            {
                POLines PostPOLines = new POLines();
                PostPOLines.Lines = LineList;
                PostPOLines.SiteID = _Site;
                PostPOLines.OrderRef = _OrderReference;

                var client = new HttpClient();
                client.MaxResponseContentBufferSize = 256000;
                client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

                var jstring = JsonConvert.SerializeObject(PostPOLines);
                var content = new StringContent(jstring, Encoding.UTF8, "application/json");
                var response = client.PostAsync(Constants.SendPOUrl, content).Result;
                var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);

                if (result.Response == true)
                {
                    await DisplayAlert("Successful", "Purchase Order Successfully Send", "Ok");
                    App.Current.MainPage = new NavigationPage(new Dashboard(user));
                }
                else
                {
                    await DisplayAlert("Send Failed", result.Message, "Ok");
                }
            }
            else
            {
                await DisplayAlert("Alert", "Can not send as there are no Products", "Ok");
            }
        }
    }
}