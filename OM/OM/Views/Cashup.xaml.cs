using Newtonsoft.Json;
using OM.Data;
using OM.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Cashup : ContentPage
	{
        public ObservableCollection<SalesT> SalesCollection { get; set; }
        public ObservableCollection<PayT> PayCollection { get; set; }
        public string username;
        public string password;
        public long NotificationID;

        public Cashup (string un, string pw, long nid)
		{
            username = un;
            password = pw;
            NotificationID = nid;
            InitializeComponent ();
            Init();
            GetCashup();
            BindingContext = new CashupList();
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
        }

        public async void GetCashup()
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var response = await client.GetAsync(Constants.Cashup + "?NotificationID=" + NotificationID);

            var cashupJson = await response.Content.ReadAsStringAsync();
            CashupList ObjList = new CashupList();
            if (cashupJson != "")
            {
                ObjList = JsonConvert.DeserializeObject<CashupList>(cashupJson);
            }
            Console.WriteLine("Cashup Json: " + cashupJson);
            //---- Sales ----//
            SalesCollection = new ObservableCollection<SalesT>(ObjList.SalesT);
            var salessorted = SalesCollection.OrderBy(x => x.Name).ToList();
            SalesListView.ItemsSource = salessorted;

            //---- Payments ----//
            PayCollection = new ObservableCollection<PayT>(ObjList.PayT);
            var paysorted = PayCollection.OrderBy(x => x.Name).ToList();
            PaymentsListView.ItemsSource = paysorted;

            //---- Totals ----//
            //ExpectedTillText.Text = ObjList.ExpectedTill.ToString();
            ActualTillActualText.Text = ObjList.ActualTillActual.ToString();
            ActualTillEposText.Text = ObjList.ActualTillEpos.ToString();
            ActualTillVarianceText.Text = ObjList.ActualTillVariance.ToString();
            NotesText.Text = ObjList.CashupNotes;
            CashupByText.Text = ObjList.CashupBy;
            CashupDateText.Text = ObjList.CashupDate.ToString("dd/MM/yyyy HH:mm");


            
            
        }


    }
}