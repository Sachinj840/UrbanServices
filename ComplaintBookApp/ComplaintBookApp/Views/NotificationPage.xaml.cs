using Acr.UserDialogs;
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
    public partial class NotificationPage : ContentPage
    {
        public NotificationPageViewModel notificationPageVM;
        public NotificationPage(string type)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                notificationPageVM = new NotificationPageViewModel(Navigation,type);
                BindingContext = notificationPageVM;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}