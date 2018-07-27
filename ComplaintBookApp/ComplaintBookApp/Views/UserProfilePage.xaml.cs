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
    public partial class UserProfilePage : ContentPage
    {
        public UserProfileViewModel userProfileVM;
        public UserProfilePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            userProfileVM = new UserProfileViewModel(Navigation);
            BindingContext = userProfileVM;
        }

        private void BindablePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (userProfileVM.IsLoadCity == true)
            {
                userProfileVM.IsLoadCity = false;
                return;
            }
            var stateIndex = Convert.ToInt32(StatePicker.SelectedIndex) + 1;
            var lstCity = userProfileVM.GetCityData(stateIndex.ToString());
            if (stateIndex > 0)
            {
                CityPicker.ItemsSource = lstCity;
                // CityPicker.IsEnabled = true;
            }
        }
    }
}