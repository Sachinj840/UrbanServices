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
	public partial class SubBannerOneUploadPage : ContentPage
    {
        public SubBannerOneUploadPageViewModel subbannerOneUploadPageVM;
        public SubBannerOneUploadPage()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            subbannerOneUploadPageVM = new SubBannerOneUploadPageViewModel(Navigation);
            BindingContext = subbannerOneUploadPageVM;
        }
	}
}