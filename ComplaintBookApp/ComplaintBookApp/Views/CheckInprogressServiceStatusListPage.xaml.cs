using Acr.UserDialogs;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Model;
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
    public partial class CheckInprogressServiceStatusListPage : ContentPage
    {
        public CheckApprovedServiceStatusListPageViewModel checkApprovedServiceStatusPageVM;
        public CheckInprogressServiceStatusListPage(ApplicationActivity pageType)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                checkApprovedServiceStatusPageVM = new CheckApprovedServiceStatusListPageViewModel(Navigation, pageType);
                BindingContext = checkApprovedServiceStatusPageVM;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        //private void listView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        //{
        //    var data = e.ItemData as ApproveServiceModel;
        //    GoToApproveComplaintDetailPage(data);
        //}
        //public async void GoToApproveComplaintDetailPage(ApproveServiceModel data)
        //{
        //    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
        //    if (data == null)
        //    {
        //        return;
        //    }
        //    Cache.goToBackButtonText = "StatusComplaintPage";
        //    await Navigation.PushAsync(new ApproveServiceComplaintDetailPage(data , ApplicationActivity.CheckInprogressServiceStatusListPage));
        //    //UserDialogs.Instance.HideLoading();
        //}
    }
}