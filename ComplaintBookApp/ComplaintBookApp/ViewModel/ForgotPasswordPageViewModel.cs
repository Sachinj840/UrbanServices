using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Views;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class ForgotPasswordPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public ForgotPasswordPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Forgot Password";
            IsBackButtonVisible = false;
            IsMenuVisible = false;
            ResetPasswordCommand = new DelegateCommand(ExecuteOnResetPassword);
            NavigateToLoginCommand = new DelegateCommand(NavigateToLogin);
        }
        #endregion

        #region Properties       

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delegate Commonds
        public DelegateCommand ResetPasswordCommand { get; set; }
        public DelegateCommand NavigateToLoginCommand { get; set; }
        #endregion

        #region Methods
        private async void ExecuteOnResetPassword(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsLoginValidation();

                    if (isValidate)
                    {
                        UserDialogs.Instance.ShowLoading("Please Wait...");
                        HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_User_ResetPassword, Email), String.Empty);
                        var response = await apicall.GetResponse<ResetPasswordResponseModel>();
                        if (response != null)
                        {
                            if (response.message.Equals("Message Sent"))
                            {
                                UserDialogs.Instance.Loading().Hide();
                                await Application.Current.MainPage.DisplayAlert(response.message, response.data, "OK");
                                await _navigation.PushAsync(new LoginPage());
                            }
                            else
                            {
                                Email = String.Empty;
                                UserDialogs.Instance.Loading().Hide();
                                await Application.Current.MainPage.DisplayAlert(response.message, response.data, "OK");
                                return;
                            }
                        }
                        UserDialogs.Instance.Loading().Hide();
                        await _navigation.PushAsync(new LoginPage());
                    }
                }
                else
                {
                    UserDialogs.Instance.Loading().Hide();
                    await Application.Current.MainPage.DisplayAlert("Network", AppConstant.NETWORK_FAILURE, "OK");
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private async void NavigateToLogin(object parma)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
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

        private async Task<bool> IsLoginValidation()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("ForgotPassword Error", "Enter email address", "OK");
                return false;
            }
            else if (!Helper.IsEmailValid(Email.ToLower()))
            {
                await Application.Current.MainPage.DisplayAlert("ForgotPassword Error", "Enter valid email address", "OK");
                return false;
            }
            return true;
        }
        #endregion
    }
}
