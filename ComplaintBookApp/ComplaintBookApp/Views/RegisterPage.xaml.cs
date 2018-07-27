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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPageViewModel regVM;
        public RegisterPage()
        {            
            InitializeComponent();            
            NavigationPage.SetHasNavigationBar(this, false);            
            regVM = new RegisterPageViewModel(Navigation);
            BindingContext = regVM;
        }

        private void BindablePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stateIndex = Convert.ToInt32(StatePicker.SelectedIndex) + 1;
            var lstCity = regVM.GetCityData(stateIndex.ToString());
            if (stateIndex > 0 )
            {
                CityPicker.ItemsSource = lstCity;
                CityPicker.IsEnabled = true;
            }            
        }
    }
}