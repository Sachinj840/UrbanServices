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
    public partial class UserNotificationPage : ContentPage
    {
        public UserNotificationPageViewModel userNotificationVM;
        public UserNotificationPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            userNotificationVM = new UserNotificationPageViewModel(Navigation);
            BindingContext = userNotificationVM;
        }
    }
}