using Newtonsoft.Json;
using OM.Data;
using OM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
        public DeviceToken deviceToken = new DeviceToken();
        public string token;

        public LoginPage ()
		{
			InitializeComponent ();
            Init();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            Lbl_Username.TextColor = Constants.MainTextColor;
            Lbl_Password.TextColor = Constants.MainTextColor;
            ActivitySpinner.IsVisible = false;
            LoginIcon.HeightRequest = Constants.LoginIconHeight;

            Entry_Username.Completed += (s, e) => Entry_Password.Focus();
            Entry_Password.Completed += (s, e) => SignInProcedure(s, e);
        }

        async void SignInProcedure(object sender, EventArgs e)
        {
            User user = new User(Entry_Username.Text, Entry_Password.Text);

            // Create an HttpClientHandler object and set to use default credentials
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            ActivitySpinner.IsVisible = true;

            if (Device.RuntimePlatform == Device.Android)
            {
                token = App.Current.Properties["token"].ToString();
                Console.WriteLine("Application token: " + token);
                deviceToken.Token = token;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                token = App.Current.Properties["tkn"].ToString();
                Console.WriteLine("Application tokenios: " + token);
                deviceToken.Token = token;
            }

            

            var client = new HttpClient(handler);
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Username + ":" + user.Password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            
            try 
            {
                var response = await client.GetStringAsync(Constants.LoginUrl);
                Console.WriteLine("Login Response: " + response);
                Tokens result = JsonConvert.DeserializeObject<Tokens>(response);
                if (result != null)
                {
                    await DisplayAlert("Login", "Login Success", "Ok");
                    ActivitySpinner.IsVisible = false;
                    App.CredentialsServce.SaveCredentials(user.Username, user.Password, token);
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        Application.Current.MainPage = new NavigationPage(new Dashboard(user));
                    }
                    else if (Device.RuntimePlatform == Device.iOS)
                    {
                        await Navigation.PushModalAsync(new NavigationPage(new Dashboard(user)));
                    }
                }
                else
                {
                    await DisplayAlert("Login", "Login Failed, username and password do not match", "Ok");
                    ActivitySpinner.IsVisible = false;
                }
                
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", ex.Message);
                await DisplayAlert("Login", "Login Error, Please try again later", "Ok");
                ActivitySpinner.IsVisible = false;
            }
        }

    }
}