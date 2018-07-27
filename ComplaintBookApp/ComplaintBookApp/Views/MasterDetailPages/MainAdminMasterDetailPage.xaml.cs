using ComplaintBookApp.Constants;
using ComplaintBookApp.ViewModel.MasterDetailViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintBookApp.Views.MasterDetailPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainAdminMasterDetailPage : MasterDetailPage
    {
        public MainAdminMasterDetailViewModel mainAdminMasterVM;
        public MainAdminMasterDetailPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                mainAdminMasterVM = new MainAdminMasterDetailViewModel(Navigation);
                BindingContext = mainAdminMasterVM;
                Cache.MasterPage = this;

                var nav = new NavigationPage
                {
                    Title = "Detail"
                };
                nav.PushAsync(new MainAdminHomePage() { Title = "Home" });
                nav.BarBackgroundColor = Color.Black;
                nav.BarTextColor = Color.Orange;
                nav.HeightRequest = 10;
                this.Detail = nav;
            }
            catch (Exception ex)
            {

            }
        }
    }
}