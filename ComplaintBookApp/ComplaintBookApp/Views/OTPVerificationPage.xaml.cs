using ComplaintBookApp.Model.RequestModels;
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
    public partial class OTPVerificationPage : ContentPage
    {
        public OTPVerificationPageViewModel otpVerificationVM;
        public OTPVerificationPage(RegisterReqModel regData)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            otpVerificationVM = new OTPVerificationPageViewModel(Navigation, regData);
            BindingContext = otpVerificationVM;
        }
    }
}