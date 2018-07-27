using Acr.UserDialogs;
using ComplaintBookApp.Common.Enumerators;
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
    public partial class ApproveServiceComplaintDetailPage : ContentPage
    {
        public ApproveServiceComplaintDetailPageViewModel approveServiceDetailPageVM;
        public ApproveServiceComplaintDetailPage(ApproveServiceModel data, ApplicationActivity pageType)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                approveServiceDetailPageVM = new ApproveServiceComplaintDetailPageViewModel(Navigation,data,pageType);
                BindingContext = approveServiceDetailPageVM;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        private void OnDateOfBirthFocused(object sender, FocusEventArgs e)
        {
            DateOfBirthEntry.IsVisible = false;
            DOBDatePicker.IsVisible = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                DateOfBirthEntry.Unfocus();
                DOBDatePicker.Focus();
                DateOfBirthEntry.Keyboard = null;
            });
        }
    }
}