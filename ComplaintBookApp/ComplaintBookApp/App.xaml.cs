using Acr.UserDialogs;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Views;
using ComplaintBookApp.Views.MasterDetailPages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ComplaintBookApp
{
    public partial class App : Application
    {
        #region Constructor
        public App()
        {
            InitializeComponent();
            if (CrossConnectivity.Current.IsConnected)
            {
                //UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                if (string.IsNullOrEmpty(Settings.AccessTokenSettings))
                {
                    MainPage = Settings.IsFirstTimeLoginSettings == true ? new NavigationPage(new LoginPage()) : new NavigationPage(new HomePage());
                }
                else
                {
                    MainPage = Settings.DisplayEmailSettings == "urbanservices.care@gmail.com" ? new NavigationPage(new MainAdminMasterDetailPage()) : new NavigationPage(new MainMasterDetailPage());
                }
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
            
        }

        #endregion

        #region parameter
        public static bool WasHttpFailed { get; set; }
        public static bool IsNavigating { get; set; }
        public static bool IsAscentSummaryPage { get; set; }

        #endregion

        #region Methods
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #endregion
    }
}
