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
    public partial class HomePage : ContentPage
    {
        public HomePageViewModel homePageVM;
        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            homePageVM = new HomePageViewModel(Navigation);
            BindingContext = homePageVM;
        }
    }
}