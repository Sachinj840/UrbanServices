using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Views;
using ComplaintBookApp.Views.MasterDetailPages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class HomePageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public HomePageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Home";
            IsBackButtonVisible = false;
            IsMenuVisible = false;
            RegisterCommand = new DelegateCommand(ExecuteOnRegister);
            LoginCommand = new DelegateCommand(ExecuteOnLogin);
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region Property

        private int _position;
        public int Position { get { return _position; } set { _position = value; OnPropertyChanged(); } }

        #endregion

        #region Delegate Commonds
        public DelegateCommand RegisterCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }

        #endregion

        #region Methods
        private async void ExecuteOnRegister(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "HomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new RegisterPage());
                    //await _navigation.PushAsync(new OTPVerificationPage(""));
                    //UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Network", AppConstant.NETWORK_FAILURE, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
            
        }

        private async void ExecuteOnLogin(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "HomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new LoginPage());
                    UserDialogs.Instance.HideLoading();
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Network", AppConstant.NETWORK_FAILURE, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
           
        }
        #endregion
    }
}
