using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Views;
using ComplaintBookApp.Views.MasterDetailPages;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class LoginPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public LoginPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Login";
            IsBackButtonVisible = false;
            IsMenuVisible = false;
            SignUpCommand = new DelegateCommand(ExecuteOnSignUp);
            LoginCommand = new DelegateCommand(ExecuteOnLoin);
            ForgotPasswordCommand = new DelegateCommand(ExecuteOnForgotPassword);
            LoginRequestModel = new LoginRequestModel();
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region Properties       

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }
        private LoginRequestModel _loginRequestModel;

        public LoginRequestModel LoginRequestModel
        {
            get { return _loginRequestModel; }
            set { _loginRequestModel = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delegate Commonds
        public DelegateCommand SignUpCommand { get; set; }
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ForgotPasswordCommand { get; set; }
        #endregion

        #region Methods
        private async void ExecuteOnLoin(object parma)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                var isValidate = await IsLoginValidation();

                if (isValidate)
                {
                    LoginRequestModel.u = Email;
                    LoginRequestModel.p = Password;
                    UserDialogs.Instance.ShowLoading("Authenticating...");
                    HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_JwtAuth_login, string.Empty);
                    var loginjson = JsonConvert.SerializeObject(LoginRequestModel);                    
                    var response = await apicall.Login<LoginResultResponseModel>(loginjson);
                    if (response != null)
                    {
                        if (response.accessToken != null && response.renewalToken != null)
                        {
                            Settings.AccessTokenSettings = response.accessToken;
                            Settings.RenewalTokenSettings = response.renewalToken;
                            Settings.DisplayNameSettings = response.displayName;
                            Cache.GlobalEmail = Email;
                            Settings.DisplayEmailSettings = Email;

                            if (Email == "urbanservices.care@gmail.com")
                            {
                                //code to go to main admin home page
                                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                                await _navigation.PushAsync(new MainAdminMasterDetailPage());
                            }
                            else
                            {
                                //code to go to main home page
                                await _navigation.PushAsync(new MainMasterDetailPage());
                            }

                            DisposeObject();
                            UserDialogs.Instance.HideLoading();
                            return;
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await UserDialogs.Instance.AlertAsync(AppConstant.LOGIN_FAILURE, "Login", "OK");
                            return;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync(AppConstant.LOGIN_FAILURE, "Login", "OK");
                        return;
                    }                    
                }
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert("Network", AppConstant.NETWORK_FAILURE, "OK");
                return;
            }
            UserDialogs.Instance.HideLoading();
        }
        private async void ExecuteOnForgotPassword(object parma)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                Cache.goToBackButtonText = "LoginPage";
                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                await _navigation.PushAsync(new ForgotPasswordPage());
                UserDialogs.Instance.HideLoading();
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                await Application.Current.MainPage.DisplayAlert("Network", AppConstant.NETWORK_FAILURE, "OK");
                return;
            }
        }
        private async void ExecuteOnSignUp(object parma)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                await _navigation.PushAsync(new RegisterPage());
                //UserDialogs.Instance.HideLoading();
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
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Login Error", "Enter both email and password.", "OK");
                return false;
            }
            else if (!Helper.IsEmailValid(Email.ToLower()))
            {
                await Application.Current.MainPage.DisplayAlert("Login Error", "Enter valid email address", "OK");
                return false;
            }
            return true;
        }
        private void DisposeObject()
        {
            LoginRequestModel = new LoginRequestModel();
        }

        #endregion
    }
}
