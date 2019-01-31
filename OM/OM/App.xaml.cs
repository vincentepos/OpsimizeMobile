using OM.Data;
using OM.Models;
using OM.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Auth;
using System.Linq;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace OM
{
	public partial class App : Application
	{
        static RestGetToken restServiceToken;
        static RestGetPO restServicePO;
        static UserDatabaseController userDatabase;
        private static Label labelScreen;
        private static bool hasInternet;
        private static Page currentpage;
        public static Timer timer;
        private static bool noInterShow;

        public static string AppName { get { return "StoreAccountInfoApp"; } }

        public static ICredentialsService CredentialsServce { get; private set; }

        public App ()
		{
            CredentialsServce = new CredentialsService();
            if (CredentialsServce.DoCredentialsExist())
            {
                User user = new User(CredentialsServce.UserName, CredentialsServce.Password);
                Console.WriteLine("Username: " + user.Username + ", Password: " + user.Password);
                // have to push the device token back to mendix here
                var account = AccountStore.Create().FindAccountsForService(App.AppName).FirstOrDefault();
                if (account.Properties.ContainsKey("license"))
                {
                    if (CredentialsServce.License == "Cashup")
                    {
                        MainPage = new NavigationPage(new Dashboard2(user));
                    }
                    else
                    {
                        MainPage = new NavigationPage(new Dashboard(user));
                    }
                }
                else
                {
                    MainPage = new LoginPage();
                }
                
            }
            else
            {
                MainPage = new LoginPage();
            }

            InitializeComponent();

			
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public static UserDatabaseController UserDatabase
        {
            get
            {
                if (userDatabase == null)
                {
                    userDatabase = new UserDatabaseController();
                }
                return userDatabase;
            }
        }

        public static RestGetToken RestServiceToken
        {
            get
            {
                if (restServiceToken == null)
                {
                    restServiceToken = new RestGetToken();
                }
                return restServiceToken;
            }
        }

        public static RestGetPO RestServicePO
        {
            get
            {
                if (restServicePO == null)
                {
                    restServicePO = new RestGetPO();
                }
                return restServicePO;
            }
        }

        //---------------------internet connection-----------------------//
        public static void StartCheckIfInternet(Label label, Page page)
        {
            labelScreen = label;
            label.Text = Constants.NoInternetText;
            label.IsVisible = false;
            hasInternet = true;
            currentpage = page;

            if(timer == null)
            {
                timer = new Timer((e) =>
                {
                    checkIfInternetOverTime();
                }, null, 10, (int)TimeSpan.FromSeconds(3).TotalMilliseconds);
            }
        }

        private static void checkIfInternetOverTime()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckInternetConnection();

            if(!networkConnection.IsConnceted)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (hasInternet)
                    {
                        if (!noInterShow)
                        {
                            hasInternet = false;
                            labelScreen.IsVisible = true;
                            await ShowDisplayAlert();
                        }
                    }
                });
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    hasInternet = true;
                    labelScreen.IsVisible = false;
                });
            }
        }

        public static bool CheckIfInternet()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckInternetConnection();
            return networkConnection.IsConnceted;
        }

        public static async Task<bool> CheckIfInternetAlertAsync()
        {
            var networkConnection = DependencyService.Get<INetworkConnection>();
            networkConnection.CheckInternetConnection();

            if (!networkConnection.IsConnceted)
            {
                if (!noInterShow)
                {
                    await ShowDisplayAlert();
                }
                return false;
            }
            return true;
        }

        private static async Task ShowDisplayAlert()
        {
            noInterShow = false;
            await currentpage.DisplayAlert("Internet","Device has no internet connection","Ok");
        }
    }
}
