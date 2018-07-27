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
    public class OTPVerificationPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private RegisterReqModel _regData = null;
        #endregion

        #region Constructor
        public OTPVerificationPageViewModel(INavigation navigation, RegisterReqModel regData) : base(navigation)
        {
            _regData = regData;
            _navigation = navigation;
            PageHeaderText = "OTP Verification";
            IsBackButtonVisible = false;
            IsMenuVisible = false;
            SubmitCommand = new DelegateCommand(ExecuteOnSubmit);
            ResendOTPCommand = new DelegateCommand(ExecuteOnResendOTP);
            GoToHomeCommand = new DelegateCommand(ExecuteOnGoToHome);
            LoginRequestModel = new LoginRequestModel();
        }
        #endregion

        #region Property
        private LoginRequestModel _loginRequestModel;

        public LoginRequestModel LoginRequestModel
        {
            get { return _loginRequestModel; }
            set { _loginRequestModel = value; OnPropertyChanged(); }
        }

        private string _verificationCode;
        public string VerificationCode { get { return _verificationCode; } set { _verificationCode = value; OnPropertyChanged(); } }

        #endregion

        #region Delegate Commonds
        public DelegateCommand SubmitCommand { get; set; }
        public DelegateCommand ResendOTPCommand { get; set; }
        public DelegateCommand GoToHomeCommand { get; set; }

        #endregion

        #region Methods       
        private async void ExecuteOnSubmit(object parma)
        {
            try
            {
                RegisterReqModel reqregister = new RegisterReqModel();
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsCheckValidation();

                    if (isValidate)
                    {
                        if (Cache.globalOTP == VerificationCode)
                        {
                            Cache.globalOTP = string.Empty;
                            UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);

                            HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_User_Register, string.Empty);
                            reqregister.FirstName = _regData.FirstName;
                            reqregister.LastName = _regData.LastName;
                            reqregister.Email = _regData.Email;
                            reqregister.password = _regData.password;
                            reqregister.Gender = _regData.Gender;
                            reqregister.State = _regData.State;
                            reqregister.City = _regData.City;
                            reqregister.MobileNo = _regData.MobileNo;
                            reqregister.Address = _regData.Address;
                            reqregister.PinCode = _regData.PinCode;

                            var regjson = JsonConvert.SerializeObject(reqregister);
                            var response = await apicall.Post<RegistrationResponseModel>(regjson);

                            if (response != null)
                            {
                                if (response.regStatus == "successful")
                                {
                                    Settings.IsFirstTimeLoginSettings = true;
                                    await Application.Current.MainPage.DisplayAlert("Registration", "Thank you for registration.", "OK");
                                    LoginRequestModel.u = _regData.Email;
                                    LoginRequestModel.p = _regData.password;
                                    UserDialogs.Instance.ShowLoading("Loading...");
                                    HttpClientHelper _apicall = new HttpClientHelper(ApiUrls.Url_JwtAuth_login, string.Empty);
                                    var loginjson = JsonConvert.SerializeObject(LoginRequestModel);
                                    var _response = await _apicall.Post<LoginResultResponseModel>(loginjson);
                                    if (_response != null)
                                    {
                                        if (_response.accessToken != null && _response.renewalToken != null)
                                        {
                                            Settings.AccessTokenSettings = _response.accessToken;
                                            Settings.RenewalTokenSettings = _response.renewalToken;
                                            Settings.DisplayNameSettings = _response.displayName;
                                            Cache.GlobalEmail = _regData.Email;
                                            Settings.DisplayEmailSettings = _regData.Email;

                                            if (_regData.Email == "urbanservices.care@gmail.com")
                                            {
                                                //code to go to main admin home page
                                                await _navigation.PushAsync(new MainAdminMasterDetailPage());
                                            }
                                            else
                                            {
                                                //code to go to main home page
                                                await _navigation.PushAsync(new MainMasterDetailPage());
                                            }
                                           
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
                                        await Application.Current.MainPage.DisplayAlert("LoginError", "Something wrong...", "OK");
                                    }
                                    UserDialogs.Instance.HideLoading();
                                }
                                else
                                {
                                    UserDialogs.Instance.HideLoading();
                                    await Application.Current.MainPage.DisplayAlert("Registration", response.message, "OK");
                                    return;
                                }
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await Application.Current.MainPage.DisplayAlert("Registration", response.message, "OK");
                                return;
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Verfication Error", "Entered code doesn't match.", "OK");
                        }
                        UserDialogs.Instance.HideLoading();
                    }
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
                await Application.Current.MainPage.DisplayAlert("Verfication Error", ex.Message, "OK");
            }

        }
        private async void ExecuteOnResendOTP(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.globalOTP = string.Empty;
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    //call api to send otp on mailid                                
                    HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_User_SendOTP, _regData.Email), string.Empty);
                    var _response = await _apicall.Get<OTPResponseModel>();
                    if (_response != null)
                    {
                        if (_response.Count > 0)
                        {
                            Cache.globalOTP = _response.Select(r => r.OTPPassword).FirstOrDefault();
                            await Application.Current.MainPage.DisplayAlert("Verfication", "OTP sent to your email id " + _regData.Email + " successfully.", "OK");
                        }
                    }
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
                await Application.Current.MainPage.DisplayAlert("Verfication Error", ex.Message, "OK");
            }

        }

        private async void ExecuteOnGoToHome(object para)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new HomePage());
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
            }
        }
        private async Task<bool> IsCheckValidation()
        {
            if (string.IsNullOrWhiteSpace(VerificationCode))
            {
                await Application.Current.MainPage.DisplayAlert("Verfication Error", "Verification code required.", "OK");
                return false;
            }
            return true;
        }
        #endregion
    }
}
