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
    public partial class SubBannerTwoUploadPage : ContentPage
    {
        public SubBannerTwoUploadPageViewModel subbannerTwoUploadPageVM;
        public SubBannerTwoUploadPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            subbannerTwoUploadPageVM = new SubBannerTwoUploadPageViewModel(Navigation);
            BindingContext = subbannerTwoUploadPageVM;
        }
    }
}