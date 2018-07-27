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
    public partial class UserInfoListPage : ContentPage
    {        
        public UserInfoListPageViewModel userinfoVM;
        public UserInfoListPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            userinfoVM = new UserInfoListPageViewModel(Navigation);
            BindingContext = userinfoVM;
        }
    }
}