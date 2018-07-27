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
    public class UserProfileViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public UserProfileViewModel(INavigation navigation) : base(navigation)
        {
            IsLoadCity = false;
            BindPickersWithData();            
            _navigation = navigation;
            PageHeaderText = "User Profile";
            IsBackButtonVisible = true;
            IsMenuVisible = false;            
            EditCommand = new DelegateCommand(ExecuteOnUpdate);
        }
        #endregion

        #region Properties               

        private bool _isLoadCity;
        public bool IsLoadCity
        {
            get { return _isLoadCity; }
            set { _isLoadCity = value; OnPropertyChanged(); }
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
        public DelegateCommand EditCommand { get; set; }
        public object RegisterReq { get; private set; }

        #endregion

        #region Methods
        private async void BindPickersWithData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);

                    //add code to get dropdown list table data               
                    HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetDropDownListData), Settings.AccessTokenSettings);
                    var response = await apicall.GetResponse<ListResponseModel>();
                    if (response != null)
                    {
                        //Country = response.country.Select(c => c.countryName).ToList();
                        State = response.state.Select(c => c.stateName).ToList();
                        //City = response.city.Select(c => c.cityName).ToList();
                        Cache.GlobalCity = response.city.Select(c => new US_CITY
                        {
                            cityId = c.cityId,
                            cityName = c.cityName,
                            state_id = c.state_id
                        }).ToList();
                    }

                    List<string> genderList = new List<string> { "Male", "Female" };
                    Genders = genderList;

                    //add code to get other data               
                    HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_UserProfileData, -1), Settings.AccessTokenSettings);
                    var _response = await _apicall.GetResponse<List<RegisterReqModel>>();
                    if (_response != null)
                    {
                        foreach (var item in _response.ToList())
                        {
                            IsLoadCity = true;
                            //State = item.lstState.Select(c => c.stateName).ToList();
                            int stateId = item.lstState.Where(s => s.stateName == item.State).Select(s => s.stateId).FirstOrDefault();
                            //stateId = stateId;// + 1; // as state start with 0                        
                            City = item.lstCity.Where(c => c.state_id == stateId).Select(c => c.cityName).ToList();
                            FirstName = item.FirstName;
                            LastName = item.LastName;
                            Email = item.Email;
                            if (!string.IsNullOrWhiteSpace(item.Gender))
                                SelectedGender = item.Gender;

                            SelectedCity = item.City;
                            SelectedState = item.State;
                            PinCode = item.PinCode;
                            MobileNumber = item.MobileNo;
                            Address = item.Address;
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
                UserDialogs.Instance.HideLoading();
            }
        }

        public List<string> GetCityData(string stateId)
        {
            return Cache.GlobalCity.Where(s => s.state_id == Convert.ToInt32(stateId)).Select(c => c.cityName).ToList();
        }
        private async void ExecuteOnUpdate(object obj)
        {
            try
            {
                RegisterReqModel RegisterReq = new RegisterReqModel();
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsEditValidation();

                    if (isValidate)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);                                               

                        HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_UpdateUserProfileData, string.Empty);
                        RegisterReq.FirstName = FirstName;
                        RegisterReq.LastName = LastName;
                       // RegisterReq.Email = Email;
                        RegisterReq.MobileNo = MobileNumber;                        
                        RegisterReq.State = SelectedState;
                        RegisterReq.Gender = SelectedGender;
                        RegisterReq.Address = Address;
                        RegisterReq.City = SelectedCity;
                        RegisterReq.PinCode = PinCode;

                        var regjson = JsonConvert.SerializeObject(RegisterReq);
                        var response = await apicall.Post<RegistrationResponseModel>(regjson);

                        if (response != null)
                        {
                            if (response.regStatus == "successful")
                            {
                                //redirect to same page              
                                await Application.Current.MainPage.DisplayAlert("Update Profile", response.message, "OK");
                                await _navigation.PushAsync(new MainMasterDetailPage());
                                UserDialogs.Instance.HideLoading();
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await Application.Current.MainPage.DisplayAlert("Update Profile", response.message, "OK");
                                return;
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Update Profile", response.message, "OK");
                            return;
                        }
                        UserDialogs.Instance.HideLoading();
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
                UserDialogs.Instance.HideLoading();
            }
        }

        private async Task<bool> IsEditValidation()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Firstname required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(LastName))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Lastname required.", "OK");
                return false;
            }                        
            else if (string.IsNullOrWhiteSpace(SelectedGender))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Select gender field.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedState))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Select state field.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedCity))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Select city field.", "OK");
                return false;
            }                      
            else if (string.IsNullOrWhiteSpace(MobileNumber))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Mobile number required.", "OK");
                return false;
            }
            else if (MobileNumber.Length < 10)
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Mobile number should be 10 digit.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(Address))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Address required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(PinCode))
            {
                await Application.Current.MainPage.DisplayAlert("UserProfile Error", "Pin code required.", "OK");
                return false;
            }
            return true;
        }

        #endregion
    }
}
