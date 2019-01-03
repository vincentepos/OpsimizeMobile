using OM.Data;
using OM.Models;
using OM.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace OM
{
	public partial class App : Application
	{
        static RestGetToken restServiceToken;
        static RestGetPO restServicePO;
        static UserDatabaseController userDatabase;

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
                MainPage = new NavigationPage(new Dashboard(user));
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
    }
}
