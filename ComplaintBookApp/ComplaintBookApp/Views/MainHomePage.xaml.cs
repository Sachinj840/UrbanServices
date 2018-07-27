using Acr.UserDialogs;
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
    public partial class MainHomePage : ContentPage
    {
        public MainHomePageViewModel mainHomeVM;
        public MainHomePage()
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                mainHomeVM = new MainHomePageViewModel(Navigation);
                BindingContext = mainHomeVM;
            }
            catch (Exception ex)
            {             
            }            
        }
        private void SfListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var data = e.ItemData as SubBannerOneImages;
            GoToSubBannerOneDetailPage(data);
        }

        public async void GoToSubBannerOneDetailPage(SubBannerOneImages data)
        {
            UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
            if (data == null)
            {
                return;
            }
            switch (data.SubOneImagelabel)
            {
                case "AC":
                    Cache.globalProduct = "AC";
                    Cache.globalCatagory = "Electrical";
                    break;
                case "Fridge":
                    Cache.globalProduct = "Fridge";
                    Cache.globalCatagory = "Electronics";
                    break;
                case "TV ( LCD / LED )":
                    Cache.globalProduct = "TV ( LCD / LED )";
                    Cache.globalCatagory = "Electronics";
                    break;
                case "Computer / Laptop":
                    Cache.globalProduct = "Computer / Laptop";
                    Cache.globalCatagory = "Electronics";
                    break;
                case "Washing Machine":
                    Cache.globalProduct = "Washing Machine";
                    Cache.globalCatagory = "Electronics";
                    break;
                case "Geyser":
                    Cache.globalProduct = "Geyser";
                    Cache.globalCatagory = "Electrical";
                    break;
                case "Printer":
                    Cache.globalProduct = "Printer";
                    Cache.globalCatagory = "Electronics";
                    break;
                default:
                    break;
            }
            Cache.goToBackButtonText = "MainHomePage";
            await Navigation.PushAsync(new BookServiceComplaintPage());
            UserDialogs.Instance.HideLoading();
        }

        private void SfListView_ItemTapped_1(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var data = e.ItemData as SubBannerTwoImages;
            GoToSubBannerTwoDetailPage(data);
        }
        public async void GoToSubBannerTwoDetailPage(SubBannerTwoImages data)
        {
            UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
            if (data == null)
            {
                return;
            }
            switch (data.SubTwoImagelabel)
            {
                case "Electrician":
                    Cache.globalProduct = "Electrician";
                    Cache.globalCatagory = "Daily Services";
                    break;
                case "Plumber":
                    Cache.globalProduct = "Plumber";
                    Cache.globalCatagory = "Daily Services";
                    break;
                case "Carpenter / Furnitur":
                    Cache.globalProduct = "Carpenter / Furnitur";
                    Cache.globalCatagory = "Daily Services";
                    break;
                case "Civil Engineer":
                    Cache.globalProduct = "Civil Engineer";
                    Cache.globalCatagory = "Daily Services";
                    break;
                case "Pest Control":
                    Cache.globalProduct = "Pest Control";
                    Cache.globalCatagory = "Daily Services";
                    break;
                case "Painter":
                    Cache.globalProduct = "Painter";
                    Cache.globalCatagory = "Daily Services";
                    break;
                case "Fabricator":
                    Cache.globalProduct = "Fabricator";
                    Cache.globalCatagory = "Daily Services";
                    break;
                default:
                    break;
            }
            Cache.goToBackButtonText = "MainHomePage";
            await Navigation.PushAsync(new BookServiceComplaintPage());
            UserDialogs.Instance.HideLoading();
        }
    }
}