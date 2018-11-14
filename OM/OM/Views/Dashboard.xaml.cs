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

        public Dashboard (User user)
		{
            username = user.Username;
            password = user.Password;
			InitializeComponent ();
            Init();
            User users = new User();
            users = user;
            Console.WriteLine("Username: " + users.Username);
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
