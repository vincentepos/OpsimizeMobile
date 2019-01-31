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
	public partial class Dashboard2 : ContentPage
	{
        public ObservableCollection<Notifications> YourCollection { get; set; }
        public List<Notifications> Items = new List<Notifications>();
        public string username;
        public string password;
        public DeviceToken deviceToken = new DeviceToken();
        public long NID;

        public Dashboard2 (User user)
		{
            username = user.Username;
            password = user.Password;
            InitializeComponent ();
            App.StartCheckIfInternet(lbl_NoInternet, this);
            Init();
            GetNotification();
            InitSearchBar();

            if (Device.RuntimePlatform == Device.Android)
            {
                string token = App.CredentialsServce.DToken;
                deviceToken.Token = token;
                var url = Constants.DeviceToken + "?Token=" + token;

                var client2 = new HttpClient();
                client2.MaxResponseContentBufferSize = 256000;
                client2.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client2.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client2.DefaultRequestHeaders.Add("Accept", "application/json");
                string userAndPasswordToken2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username + ":" + user.Password));
                client2.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken2}");

                Console.WriteLine("Post URL: " + url);
                var jstring = JsonConvert.SerializeObject(deviceToken);
                var content = new StringContent(jstring, Encoding.UTF8, "application/json");
                var response2 = client2.PostAsync(url, content).Result;
                var result2 = JsonConvert.DeserializeObject<GeneralResponse>(response2.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Post Result: " + response2.Content.ReadAsStringAsync().Result);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                string token = App.CredentialsServce.DToken;
                deviceToken.Token = token;
                var url = Constants.DeviceTokenIOS + "?Token=" + token;

                var client2 = new HttpClient();
                client2.MaxResponseContentBufferSize = 256000;
                client2.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
                client2.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client2.DefaultRequestHeaders.Add("Accept", "application/json");
                string userAndPasswordToken2 = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username + ":" + user.Password));
                client2.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken2}");

                Console.WriteLine("Post URL: " + url);
                var jstring = JsonConvert.SerializeObject(deviceToken);
                var content = new StringContent(jstring, Encoding.UTF8, "application/json");
                var response2 = client2.PostAsync(url, content).Result;
                var result2 = JsonConvert.DeserializeObject<GeneralResponse>(response2.Content.ReadAsStringAsync().Result);
                Console.WriteLine("Post Result: " + response2.Content.ReadAsStringAsync().Result);
            }
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            notification_search.BackgroundColor = Constants.BackgroundColor;
        }

        public async void GetNotification()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.AllNotifications);

            var notificationJson = await response.Content.ReadAsStringAsync();
            NotificationList ObjList = new NotificationList();
            if (notificationJson != "")
            {
                ObjList = JsonConvert.DeserializeObject<NotificationList>(notificationJson);
            }
            YourCollection = new ObservableCollection<Notifications>(ObjList.Notifications);
            Items = ObjList.Notifications;
            var sorted = YourCollection.OrderByDescending(x => x.SendDate).ToList();
            NotificationsListView.ItemsSource = sorted;
            NotificationsListView.ItemTapped += NotificationsListView_ItemTapped;
        }

        public void NotificationsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Notifications item = (Notifications)e.Item;
            NID = item.NotificationID;
            Navigation.PushAsync(new Cashup(username, password, NID));

            //if (item.Status == "Draft PO")
            //{
            //    Navigation.PushAsync(new EditPO(username, password, item.OrderRef, item.Status, item.Location, item.User, item.DateOrdered, item.SiteID, item.OrderValue));
            //}
            //else
            //{
            //    Navigation.PushAsync(new POView(item, username, password));
            //}

        }

        void InitSearchBar()
        {
            notification_search.TextChanged += (s, e) => FilterItem(notification_search.Text);
            notification_search.SearchButtonPressed += (s, e) => FilterItem(notification_search.Text);
        }

        private void FilterItem(string filter)
        {
            NotificationsListView.BeginRefresh();
            if (string.IsNullOrWhiteSpace(filter))
            {
                NotificationsListView.ItemsSource = Items;
            }
            else
            {
                NotificationsListView.ItemsSource = Items.Where(X => (X.Title.ToLower().Contains(filter.ToLower()) || X.SendTo.ToLower().Contains(filter.ToLower()) || X.SendDate.ToLongDateString().Contains(filter.ToLower())));
            }
            NotificationsListView.EndRefresh();
        }

        void LogoutProcedure(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
            App.CredentialsServce.DeleteCredentials();
            DisplayAlert("Logout", "Logged Out", "Ok");
        }
    }
}