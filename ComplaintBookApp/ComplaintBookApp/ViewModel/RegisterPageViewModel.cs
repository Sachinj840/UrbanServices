using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Tables;
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
    public class RegisterPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public RegisterPageViewModel(INavigation navigation) : base(navigation)
        {
            BindPickers();
            _navigation = navigation;
            PageHeaderText = "Register";
            IsBackButtonVisible = false;
            IsMenuVisible = false;
            LoginCommand = new DelegateCommand(ExecuteOnLogin);
            RegistrationCommand = new DelegateCommand(ExecuteOnRegistration);
            LoginRequestModel = new LoginRequestModel();
        }
        #endregion

        #region Properties

        private LoginRequestModel _loginRequestModel;
        public LoginRequestModel LoginRequestModel
        {
            get { return _loginRequestModel; }
            set { _loginRequestModel = value; OnPropertyChanged(); }
        }
        private string _pinCode;
        public string PinCode
        {
            get { return _pinCode; }
            set { _pinCode = value; OnPropertyChanged(); }
        }

        private List<string> _state;
        public List<string> State
        {
            get { return _state; }
            set { _state = value; OnPropertyChanged(); }
        }
        private List<string> _city;
        public List<string> City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged(); }
        }
        private string _mobileNumber;
        public string MobileNumber
        {
            get { return _mobileNumber; }
            set { _mobileNumber = value; OnPropertyChanged(); }
        }
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }
        private bool _isDateOfBirthEntryVisible;
        public bool IsDateOfBirthEntryVisible
        {
            get { return _isDateOfBirthEntryVisible; }
            set { _isDateOfBirthEntryVisible = value; OnPropertyChanged(); }
        }

        private bool _isDateOfBirthDatePickerVisible;
        public bool IsDateOfBirthDatePickerVisible
        {
            get { return _isDateOfBirthDatePickerVisible; }
            set { _isDateOfBirthDatePickerVisible = value; OnPropertyChanged(); }
        }

        private List<string> _genders;
        public List<string> Genders
        {
            get { return _genders; }
            set { _genders = value; OnPropertyChanged(); }
        }

        private string _selectedGender;
        public string SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; OnPropertyChanged(); }
        }

        private string _selectedState;
        public string SelectedState
        {
            get { return _selectedState; }
            set { _selectedState = value; OnPropertyChanged(); }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get { return _selectedCity; }
            set { _selectedCity = value; OnPropertyChanged(); }
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); }
        }

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

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delegate Commonds
        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand RegistrationCommand { get; set; }
        public DelegateCommand TermsAndConditionCommand { get; set; }

        #endregion

        #region Methods
        private async void BindPickers()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    bool isCallFirstTime = false;
                    //UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);

                    //add code to get dropdown list table data               
                    HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetDropDownListData), String.Empty);
                    var response = await apicall.GetResponse<ListResponseModel>();
                    if (response != null)
                    {
                        //Country = response.country.Select(c => c.countryName).ToList();
                        State = response.state.Select(c => c.stateName).ToList();
                        Cache.GlobalCity = response.city.Select(c => new US_CITY
                        {
                            cityId = c.cityId,
                            cityName = c.cityName,
                            state_id = c.state_id
                        }).ToList();
                    }

                    List<string> genderList = new List<string> { "Male", "Female" };
                    Genders = genderList;

                    Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                    {
                        if (isCallFirstTime == false)
                        {
                            isCallFirstTime = true;
                            UserDialogs.Instance.HideLoading();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    });
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

        public List<string> GetCityData(string stateId)
        {
            return Cache.GlobalCity.Where(s => s.state_id == Convert.ToInt32(stateId)).Select(c => c.cityName).ToList();
        }

        private async void ExecuteOnLogin(object parma)
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

        private async void ExecuteOnRegistration(object obj)
        {
            RegisterReqModel reqregister = new RegisterReqModel();
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsRegistrationValidation();

                    if (isValidate)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);

                        reqregister.FirstName = FirstName;
                        reqregister.LastName = LastName;
                        reqregister.Email = Email;
                        reqregister.password = Password;
                        reqregister.Gender = SelectedGender;
                        reqregister.State = SelectedState;
                        reqregister.City = SelectedCity;
                        reqregister.MobileNo = MobileNumber;
                        reqregister.Address = Address;
                        reqregister.PinCode = PinCode;

                        Cache.GlobalEmail = Email;
                        Cache.DisplayName = FirstName + " " + LastName;

                        HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_User_Register, string.Empty);                       

                        var regjson = JsonConvert.SerializeObject(reqregister);
                        var response = await apicall.Post<RegistrationResponseModel>(regjson);

                        if (response != null)
                        {
                            if (response.regStatus == "successful")
                            {
                                Settings.IsFirstTimeLoginSettings = true;
                                await Application.Current.MainPage.DisplayAlert("Registration", "Thank you for registration.", "OK");
                                LoginRequestModel.u = reqregister.Email;
                                LoginRequestModel.p = reqregister.password;
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
                                        Cache.GlobalEmail = reqregister.Email;
                                        Settings.DisplayEmailSettings = reqregister.Email;

                                        if (reqregister.Email == "urbanservices.care@gmail.com")
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
                        ////redirect to OTP Page with register data 

                        ////call api to send otp on mailid                                
                        //HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_User_SendOTP), string.Empty);
                        //Dictionary<string, string> dictquery = new Dictionary<string, string>();
                        //dictquery.Add("emailId", Email);
                        //var _response = await _apicall.Get<OTPResponseModel>(dictquery);
                        //if (_response != null)
                        //{
                        //    string message = "We will sent you OTP on your email id " + Email + ", Verify OTP to complete registration process.";
                        //    await Application.Current.MainPage.DisplayAlert("Registration", message , "OK");
                        //    Cache.globalOTP = _response.OTPPassword;
                        //    UserDialogs.Instance.HideLoading();
                        //    //redirect to OTP page                             
                        //    await _navigation.PushAsync(new OTPVerificationPage(reqregister));
                        //}
                        //else
                        //{
                        //    UserDialogs.Instance.HideLoading();
                        //    await Application.Current.MainPage.DisplayAlert("Registration", "Error sending OTP", "OK");
                        //}                        
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        return;
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
                await Application.Current.MainPage.DisplayAlert("Registration Error", ex.Message, "OK");
                UserDialogs.Instance.HideLoading();
                return;
            }
        }

        private async Task<bool> IsRegistrationValidation()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Firstname required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Lastname required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Email))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Email address required.", "OK");
                return false;
            }
            else if (!Helper.IsEmailValid(Email.ToLower()))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Enter valid email address.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Password required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedGender))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Select gender field.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedState))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Select state field.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedCity))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Select city field.", "OK");
                return false;
            }
            //else if (string.IsNullOrWhiteSpace(DOB))
            //{
            //    await Application.Current.MainPage.DisplayAlert("Registration Error", "DOB required, try again.", "OK");
            //    return false;
            //}            
            else if (string.IsNullOrWhiteSpace(MobileNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Mobile number required.", "OK");
                return false;
            }
            else if (MobileNumber.Length < 10)
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Mobile number should be 10 digit.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Address))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Address required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(PinCode))
            {
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Pin code required.", "OK");
                return false;
            }
            return true;
        }

        #endregion
    }
}
