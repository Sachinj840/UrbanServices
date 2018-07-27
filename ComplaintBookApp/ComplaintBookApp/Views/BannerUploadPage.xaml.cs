using Acr.UserDialogs;
using ComplaintBookApp.Model;
using ComplaintBookApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintBookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BannerUploadPage : ContentPage
    {
        public BannerUploadPageViewModel bannerUploadPageVM;
        public BannerUploadPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            bannerUploadPageVM = new BannerUploadPageViewModel(Navigation);
            BindingContext = bannerUploadPageVM;
        }
       
    }
}