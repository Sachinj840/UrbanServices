using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
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
    public class ApproveServiceComplaintDetailPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private ApproveServiceModel _approveServiceData;
        private string _profileImage = String.Empty;
        private ApplicationActivity _pageType;
        #endregion

        #region Constructor
        public ApproveServiceComplaintDetailPageViewModel(INavigation navigation, ApproveServiceModel data, ApplicationActivity pageType) : base(navigation)
        {
            _approveServiceData = data;
            _navigation = navigation;
            _pageType = pageType;
            PageHeaderText = "Complaint Detail";
            IsMenuVisible = false;
            IsBackButtonVisible = true;           
            LoadListData();            
            IsComplaintDateEntryVisible = true;
            IsComplaintDatePickerVisible = false;
            UpdateCommand = new DelegateCommand(ExecuteOnUpdate);
            ComplaintDate = DateTime.Today;
        }
        #endregion

        #region Properties   

        private string _selectedCatagory;
        public string SelectedCatagory
        {
            get { return _selectedCatagory; }
            set { _selectedCatagory = value; OnPropertyChanged(); }
        }

        private List<string> _catagory;
        public List<string> Catagory { get { return _catagory; } set { _catagory = value; OnPropertyChanged(); } }

        private List<string> status;
        public List<string> Status { get { return status; } set { status = value; OnPropertyChanged(); } }

        private string _complaint;
        public string Complaint { get { return _complaint; } set { _complaint = value; OnPropertyChanged(); } }
        
        private string _mobileNo;
        public string MobileNo
        {
            get { return _mobileNo; }
            set { _mobileNo = value; OnPropertyChanged(); }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; OnPropertyChanged(); }
        }

        private DateTime _complaintDate;
        public DateTime ComplaintDate
        {
            get { return _complaintDate; }
            set { _complaintDate = value; OnPropertyChanged(); }
        }

        private bool _isComplaintDateEntryVisible;
        public bool IsComplaintDateEntryVisible
        {
            get { return _isComplaintDateEntryVisible; }
            set { _isComplaintDateEntryVisible = value; OnPropertyChanged(); }
        }

        private bool _isComplaintDatePickerVisible;
        public bool IsComplaintDatePickerVisible
        {
            get { return _isComplaintDatePickerVisible; }
            set { _isComplaintDatePickerVisible = value; OnPropertyChanged(); }
        }
        
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged(); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _selectedStatus;
        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set { _selectedStatus = value; OnPropertyChanged(); }
        }
        #endregion

        #region Delegate Commonds                     
        public DelegateCommand UpdateCommand { get; set; }
        #endregion

        #region Methods                    
        private async void LoadListData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClientHelper apicall = null;
                IsComplaintDateEntryVisible = false;
                IsComplaintDatePickerVisible = true;

                List<string> catagory = new List<string> { "Electronics", "Electrical", "IT", "Daily Services" };
                Catagory = catagory;

                List<string> status = new List<string> { "Pending", "Approved", "InProgress", "Resolved" };
                Status = status;

                switch (_pageType)
                {                   
                    case ApplicationActivity.CheckApprovedServiceStatusListPage:
                        apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPendingServiceData, _approveServiceData.complaintId,"Approved"), Settings.AccessTokenSettings);
                        break;
                    case ApplicationActivity.CheckInprogressServiceStatusListPage:
                        apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPendingServiceData, _approveServiceData.complaintId,"InProgress"), Settings.AccessTokenSettings);
                        break;
                    default:
                        apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPendingServiceData, _approveServiceData.complaintId, "Pending"), Settings.AccessTokenSettings);
                        break;
                }
                
                var response = await apicall.GetResponse<List<ApproveServiceDetailModel>>();
                if (response != null)
                {
                    if (response.Count > 0)
                    {
                        foreach (var item in response)
                        {
                            UserName = item.UserName;
                            ProductName = item.ProductName;
                            SelectedStatus = item.Status;
                            SelectedCatagory = item.Catagory;
                            Complaint = item.Complaint;
                            //ComplaintDate = item.ComplaintDate;
                            Cache.userEmail = item.EmailId;
                            Cache.GlobalUserId = item.UserId;
                            Cache.GlobalComplaintId = _approveServiceData.complaintId.ToString();
                            MobileNo = item.MobileNo;
                            Address = item.Address;

                            if (item.ComplaintDate != null)
                            {
                                ComplaintDate = Convert.ToDateTime(item.ComplaintDate);
                                IsComplaintDateEntryVisible = false;
                                IsComplaintDatePickerVisible = true;
                            }
                            else
                            {
                                IsComplaintDateEntryVisible = true;
                                IsComplaintDatePickerVisible = false;
                            }
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("Error", "Error fetching record", "OK");
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Error", "Error fetching record", "OK");
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

        private async void ExecuteOnUpdate(object obj)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsRegistrationValidation();

                    if (isValidate)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                        ApproveServiceDetailModel productReq = new ApproveServiceDetailModel();

                        HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_Approve_Complaint, Settings.AccessTokenSettings);
                        productReq.Catagory = SelectedCatagory;
                        productReq.Complaint = Complaint;
                        productReq.ProductName = ProductName;
                        productReq.ComplaintDate = ComplaintDate;
                        productReq.UserName = UserName;
                        productReq.Status = SelectedStatus;
                        productReq.UserId = Cache.GlobalUserId;
                        productReq.ComplaintId = Cache.GlobalComplaintId;

                        var regjson = JsonConvert.SerializeObject(productReq);
                        var response = await apicall.Post<RegistrationResponseModel>(regjson);

                        if (response != null)
                        {
                            if (response.regStatus == "successful")
                            {
                                await Application.Current.MainPage.DisplayAlert("Complaint Success", response.message, "OK");

                                if (SelectedStatus == "Approved")
                                {                                    
                                    string emailid = Cache.userEmail;
                                    HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_Send_EmailUser, emailid, ProductName, SelectedCatagory), Settings.AccessTokenSettings);
                                    var _response = await _apicall.GetResponse<ResetPasswordResponseModel>();
                                    if (_response != null)
                                    {
                                        if (_response.message.Equals("Message Sent"))
                                        {
                                            UserDialogs.Instance.Loading().Hide();
                                            //await Application.Current.MainPage.DisplayAlert(_response.message, _response.data, "OK");
                                            Cache.goToBackButtonText = "MainAdminHomePage";
                                            await _navigation.PushAsync(new ApproveServiceComplaintListPage());
                                            return;
                                        }
                                        else
                                        {
                                            UserDialogs.Instance.Loading().Hide();
                                            //await Application.Current.MainPage.DisplayAlert(_response.message, _response.data, "OK");
                                            Cache.goToBackButtonText = "MainAdminHomePage";
                                            await _navigation.PushAsync(new ApproveServiceComplaintListPage());
                                            return;
                                        }
                                    }
                                }
                                else if (SelectedStatus == "InProgress")
                                {
                                    UserDialogs.Instance.Loading().Hide();                                    
                                    Cache.goToBackButtonText = "MainAdminHomePage";
                                    await PageNavigation(ApplicationActivity.CheckApprovedServiceStatusListPage, ApplicationActivity.CheckApprovedServiceStatusListPage);
                                    return;
                                }
                                else
                                {
                                    UserDialogs.Instance.Loading().Hide();
                                    await _navigation.PushAsync(new MainAdminMasterDetailPage());
                                    return;
                                }
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await Application.Current.MainPage.DisplayAlert("Complaint Error", response.message, "OK");
                                return;
                            }
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Complaint Error", response.message, "OK");
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
                await Application.Current.MainPage.DisplayAlert("Complaint Error", ex.Message, "OK");
                return;
            }
        }       

        private async Task<bool> IsRegistrationValidation()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                await Application.Current.MainPage.DisplayAlert("Complaint Error", "Product name required.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedCatagory))
            {
                await Application.Current.MainPage.DisplayAlert("Complaint Error", "Select catagory field.", "OK");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(SelectedStatus))
            {
                await Application.Current.MainPage.DisplayAlert("Complaint Error", "Select status field.", "OK");
                return false;
            }

            return true;
        }
        #endregion
    }
}
