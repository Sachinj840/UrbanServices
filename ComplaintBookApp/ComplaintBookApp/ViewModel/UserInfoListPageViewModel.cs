using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Interfaces;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Tables;
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
    public class UserInfoListPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public UserInfoListPageViewModel(INavigation navigation) : base(navigation)
        {
            DownloadCommand = new DelegateCommand(ExecuteOnPdfGenerate);
            _navigation = navigation;
            PageHeaderText = "User info List";
            IsBackButtonVisible = true;
            IsMenuVisible = false;
            UserDataList = new ObservableCollection<UserDataModel>();
            UserData = new ReadOnlyObservableCollection<UserDataModel>(UserDataList);
            BindData();
        }
        #endregion

        #region Properties  

        private ReadOnlyObservableCollection<UserDataModel> userData;
        public ReadOnlyObservableCollection<UserDataModel> UserData
        {
            get { return userData; }
            set { userData = value; OnPropertyChanged(); }
        }

        private ObservableCollection<UserDataModel> _userDataList;

        public ObservableCollection<UserDataModel> UserDataList
        {
            get { return _userDataList; }
            set { _userDataList = value; OnPropertyChanged(); }
        }

        private bool _isStatusVisible;
        public bool IsStatusVisible
        {
            get { return _isStatusVisible; }
            set { _isStatusVisible = value; OnPropertyChanged(); }
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
        public string Gender
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
        public DelegateCommand DownloadCommand { get; set; }

        #endregion

        #region Methods

        private async void ExecuteOnPdfGenerate(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);                   

                    //add code to get other data               
                    HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_UserProfileData, parma), Settings.AccessTokenSettings);
                    var _response = await _apicall.GetResponse<List<RegisterReqModel>>();
                    if (_response != null)
                    {
                        StringBuilder html = new StringBuilder();
                        html.Append("<html><body>");
                        html.Append("<h1>" + "User Detail Information " + "</h1>");
                        html.Append("<table style='border: 1px solid black; border-collapse: collapse;'>");
                        foreach (var item in _response.ToList())
                        {
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>FIRST NAME : </h3></td><td><h3>" + item.FirstName + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>LAST NAME : </h3></td><td><h3>" + item.LastName + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>EMAIL : </h3></td><td><h3>" + item.Email + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>GENDER : </h3></td><td><h3>" + item.Gender + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>CITY : </h3></td><td><h3>" + item.City + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>STATE : </h3></td><td><h3>" + item.State + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>MOBILE NO : </h3></td><td><h3>" + item.MobileNo + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>ADDRESS : </h3></td><td><h3>" + item.Address + "</h3></td></tr>");
                            html.Append("<tr style='border: 1px solid black; border-collapse: collapse;'><td><h3>PIN CODE : </h3></td><td><h3>" + item.PinCode + "</h3></td></tr>");
                            html.Append("\n");                            
                                                                                  
                        }

                        html.Append("</table>");
                        html.Append("</body></html>");                        

                        // Create a source for the webview
                        var htmlSource = new HtmlWebViewSource();
                        htmlSource.Html = html.ToString();

                        // Create and populate the Xamarin.Forms.WebView
                        var browser = new WebView();
                        browser.Source = htmlSource;

                        var printService = DependencyService.Get<IPrint>();
                        printService.PrintInfo(browser);
                        
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

        private async void BindData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);                    

                    //add code to get other data               
                    HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_AllUserProfileData), Settings.AccessTokenSettings);
                    var _response = await _apicall.GetResponse<List<UserDataModel>>();
                    if (_response != null)
                    {
                        if (_response.Count > 0)
                        {
                            IsStatusVisible = false;                           
                            UserDataList = new ObservableCollection<UserDataModel>(_response);
                        }
                        else
                        {
                            IsStatusVisible = true;
                            UserDialogs.Instance.HideLoading();
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

        #endregion
    }
}
