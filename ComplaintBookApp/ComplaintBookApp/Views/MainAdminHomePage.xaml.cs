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
    public partial class MainAdminHomePage : ContentPage
    {
        public MainAdminHomePageViewModel mainAdminPageVM;
        public MainAdminHomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            mainAdminPageVM = new MainAdminHomePageViewModel(Navigation);
            BindingContext = mainAdminPageVM;
        }
    }
}