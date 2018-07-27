using ComplaintBookApp.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintBookApp.UserControls.PageHeaderControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HeaderUC : ContentView
	{
		public HeaderUC ()
		{
			InitializeComponent ();
		}
        private void OnMenuTapped(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Cache.MasterPage.IsPresented = true;
            });
        }
    }
}