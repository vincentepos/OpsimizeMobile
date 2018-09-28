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
	public partial class TakeDeliveryLine : ContentPage
	{
        public string qty;
        public DNLine line = new DNLine();

		public TakeDeliveryLine (DNLine dnline)
		{
            line = dnline;
			InitializeComponent ();
            Init();
            ViewDNLine(dnline);
		}

        void Init()
        {
            BackgroundColor = Constants.BackgroundColor;
            CodeText.TextColor = Constants.MainTextColor;
            DescriptionText.TextColor = Constants.MainTextColor;
            OrderSizeText.TextColor = Constants.MainTextColor;
            QtyText.TextColor = Constants.MainTextColor;
            ReceivedText.TextColor = Constants.MainTextColor;
        }
        public void ViewDNLine(DNLine dnline)
        {
            CodeText.Text = dnline.Code;
            DescriptionText.Text = dnline.Description;
            OrderSizeText.Text = dnline.OrderSize;
            QtyText.Text = dnline.Qty.ToString();
            ReceivedText.Text = dnline.QtyReceived.ToString();
            dnline.QtyReceived = Convert.ToDouble(qty);

            //dnline.QtyReceived += qtyChange;
        }

        void qtyChange(object sender, TextChangedEventArgs e)
        {
            qty = e.NewTextValue;
            Console.WriteLine("value of: "+qty);
        }

        async void SaveDeliveryNoteLineProcess(object sender, EventArgs e)
        {
            line.QtyReceived = Convert.ToDouble(qty);
            await Navigation.PopModalAsync();
        }

        async void CancelDeliveryNoteProcess(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}