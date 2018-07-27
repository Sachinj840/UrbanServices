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
    public partial class MainMasterDetailPage : MasterDetailPage
    {
        public MainMasterDetailViewModel mainMasterVM;
        public MainMasterDetailPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                mainMasterVM = new MainMasterDetailViewModel(Navigation);
                BindingContext = mainMasterVM;
                Cache.MasterPage = this;

                var nav = new NavigationPage
                {
                    Title = "Detail"
                };
                nav.PushAsync(new MainHomePage() { Title = "Home" });
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