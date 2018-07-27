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
    public partial class CheckServiceStatusListPage : ContentPage
    {
        public CheckServiceStatusListPageViewModel checkServiceStatusPageVM;
        public CheckServiceStatusListPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                checkServiceStatusPageVM = new CheckServiceStatusListPageViewModel(Navigation);
                BindingContext = checkServiceStatusPageVM;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}