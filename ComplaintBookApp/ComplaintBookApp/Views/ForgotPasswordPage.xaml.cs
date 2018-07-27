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
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPageViewModel forgotPasswordVM;
        public ForgotPasswordPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            forgotPasswordVM = new ForgotPasswordPageViewModel(Navigation);
            BindingContext = forgotPasswordVM;
        }
    }
}