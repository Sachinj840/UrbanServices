using Acr.UserDialogs;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Model;
using ComplaintBookApp.ViewModel;
using Syncfusion.ListView.XForms;
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
    public partial class ApproveServiceComplaintListPage : ContentPage
    {
        public ApproveServiceComplaintPageViewModel approveServicePageVM;
        public ApproveServiceComplaintListPage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                approveServicePageVM = new ApproveServiceComplaintPageViewModel(Navigation);
                BindingContext = approveServicePageVM;
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
        //    Cache.goToBackButtonText = "ApproveComplaintPage";
        //    await Navigation.PushAsync(new ApproveServiceComplaintDetailPage(data, ApplicationActivity.Pending));
        //    //UserDialogs.Instance.HideLoading();
        //}
    }
}