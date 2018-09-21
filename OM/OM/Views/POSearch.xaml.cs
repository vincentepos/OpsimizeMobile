using OM.Data;
using OM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OM.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class POSearch : ContentPage
	{
        public User user2;
        public POSearch ()
		{
			InitializeComponent ();
            Init();
        }

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            ActivitySpinner.IsVisible = false;
        }

        public async void SearchPOProcedure(object sender, EventArgs e)
        {
            PO po = new PO(SearchPO.Text);
            if (po.CheckPOInformation())
            {
                var poresult = await App.RestServicePO.GetPO(user2, po);
                if (poresult.OrderRef != null)
                {
                    DisplayAlert("Search", "Searched", "Ok");
                }
                else
                {
                    DisplayAlert("Search", "Search can not be empty", "Ok");
                }
            }
        }
    }
}