using Newtonsoft.Json;
using OM.Data;
using OM.Models;
using Plugin.Media;
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
	public partial class TakeDelivery : ContentPage
	{
        public string username;
        public string password;
        public string qty;
        public string base64;

        public DeliveryNoteLines DNLineList = new DeliveryNoteLines();

        public TakeDelivery (string item, string un, string pw)
		{
            username = un;
            password = pw;
            InitializeComponent ();
            Init();
            PopulateDNView(item);
            CameraButton.Clicked += CameraButton_Clicked;
            BindingContext = new DNLine();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
        }

        public async void PopulateDNView(string item)
        {
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

            var responseline = await client.GetAsync(Constants.DNLineUrl + "?PORef=" + item);

            var dnlineJson = await responseline.Content.ReadAsStringAsync();
            DeliveryNoteLines ObjDNLineList = new DeliveryNoteLines();
            if (dnlineJson != "")
            {
                ObjDNLineList = JsonConvert.DeserializeObject<DeliveryNoteLines>(dnlineJson);
                Console.WriteLine("Response: " + dnlineJson);
            }
            DNLinesListView.ItemsSource = ObjDNLineList.DNLines;
            DNLineList = ObjDNLineList;
            DNLineList.PORef = item;
        }


        void QtyChange(object sender, TextChangedEventArgs e)
        {
            qty = e.NewTextValue;
            Console.WriteLine("value of: " + qty);
        }

        async void UpdateChangesProcedure(object sender, EventArgs e)
        {
            //DNLineList.Base64 = base64;
            //Console.WriteLine("Base64 String: " + DNLineList.Base64);
            User loginuser = new User(username, password);
            var client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(username + ":" + password));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            
            var jstring = JsonConvert.SerializeObject(DNLineList);
            var content = new StringContent(jstring, Encoding.UTF8, "application/json");
            var response = client.PostAsync(Constants.PostDeliveryUrl, content).Result;
            var result = JsonConvert.DeserializeObject<GeneralResponse>(response.Content.ReadAsStringAsync().Result);
            Console.WriteLine("Post Result: " + response.Content.ReadAsStringAsync().Result);

            if (result.Response == true)
            {
                await DisplayAlert("Uploaded", result.Message, "Ok");
                App.Current.MainPage = new NavigationPage(new Dashboard(loginuser));
            }
            else
            {
                await DisplayAlert("Not Uploaded", result.Message, "Ok");
                await Navigation.PopModalAsync();
            }

            //await Navigation.PopModalAsync();

        }

        async void CancelChangesProcedure(object sender, EventArgs e)
        {
            await DisplayAlert("Canceled", "Delivery Line changes has been canceled.", "Ok");
            await Navigation.PopModalAsync();
        }

        async void CameraButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            //var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            //if (photo != null)
            //    PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", "No camera avaialble.", "OK");
                return;
            }

            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                await DisplayAlert("File Location", file.Path, "OK");

                PhotoImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    base64 = System.Convert.ToBase64String(bytes);
                    file.Dispose();
                    return stream;
                });
            }
            catch
            {
                await DisplayAlert("Uh oh", "Something went wrong, but don't worry we caught it!", "OK");
            }
        }
    }
}