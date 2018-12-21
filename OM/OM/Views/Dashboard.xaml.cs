using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using OM.Models;
using Newtonsoft.Json;
using OM.Data;
using System.Net.Http.Headers;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Dashboard : ContentPage
	{
        public string username;
        public string password;
        public DeviceToken deviceToken = new DeviceToken();

        public Dashboard (User user)
		{
            username = user.Username;
            password = user.Password;
			InitializeComponent ();
            Init();
            User users = new User();
            users = user;
            Console.WriteLine("Username: " + users.Username);

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
                string token = App.Current.Properties["tokenios"].ToString();

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
        }

        async void ShowPOListProcedure(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OpenPOList(username, password));
        }

        async void NewPOProcedure(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPO(username, password));
        }

        async void QuickDeliveryProcedure(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Quick Delivery not implemented yet.", "Ok");
        }

        async void DeliveryNoteProcedure(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Delivery Notes not implemented yet.", "Ok");
        }

        async void CreditNoteProcedure(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Credit Notes not implemented yet.", "Ok");
        }

        async void SupplierProcedure(object sender, EventArgs e)
        {
            await DisplayAlert("Alert", "Suppliers not implemented yet.", "Ok");
        }

        void LogoutProcedure(object sender, EventArgs e)
        {
            App.Current.MainPage = new NavigationPage(new LoginPage());
            DisplayAlert("Logout", "Logged Out", "Ok");
        }

    }
}
