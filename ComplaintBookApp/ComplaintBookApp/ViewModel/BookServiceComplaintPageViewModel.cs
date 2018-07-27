using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Views.MasterDetailPages;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class BookServiceComplaintPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public BookServiceComplaintPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Book Service/Complaint";
            IsBackButtonVisible = true;
            IsMenuVisible = false;
            BindPickers();
            IsComplaintDateEntryVisible = true;
            IsComplaintDatePickerVisible = false;
            RegisterCommand = new DelegateCommand(ExecuteOnRegistration);
            ComplaintDate = DateTime.Today;
        }
        #endregion

        #region Property

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

        private List<string> _catagory;
        public List<string> Catagory { get { return _catagory; } set { _catagory = value; OnPropertyChanged(); } }        

        private string _complaint;
        public string Complaint { get { return _complaint; } set { _complaint = value; OnPropertyChanged(); } }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; OnPropertyChanged(); }
        }

        private string _productDescription;
        public string ProductDescription
        {
            get { return _productDescription; }
            set { _productDescription = value; OnPropertyChanged(); }
        }
      
        private string _selectedCatagory;
        public string SelectedCatagory
        {
            get { return _selectedCatagory; }
            set { _selectedCatagory = value; OnPropertyChanged(); }
        }               
        #endregion

        #region Delegate Commonds
        public DelegateCommand RegisterCommand { get; set; }

        #endregion

        #region Methods
        private async void ExecuteOnRegistration(object obj)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsRegistrationValidation();

                    if (isValidate)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                        ProductComplaintReqModel productReq = new ProductComplaintReqModel();

                        HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_Register_Complaint, Settings.AccessTokenSettings);
                        productReq.catagoryName = SelectedCatagory;
                        productReq.complaintSummary = Complaint;
                        productReq.productName = ProductName;
                        productReq.complaintDate =Convert.ToDateTime(ComplaintDate.Date.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, CultureInfo.CurrentCulture));

                        var regjson = JsonConvert.SerializeObject(productReq);
                        var response = await apicall.Post<RegistrationResponseModel>(regjson);

                        if (response != null)
                        {
                            if (response.regStatus == "successful")
                            {                               
                                Cache.globalCatagory = string.Empty;
                                Cache.globalProduct = string.Empty;
                                await Application.Current.MainPage.DisplayAlert("Complaint Success", response.message, "OK");

                                EmailModel emailData = new EmailModel();
                                HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_Send_EmailAdmin), Settings.AccessTokenSettings);
                                emailData.complaintDate = ComplaintDate;
                                emailData.Service = ProductName;
                                emailData.catagory = SelectedCatagory;
                                emailData.emailId = Cache.GlobalEmail;

                                var _regjson = JsonConvert.SerializeObject(emailData);

                                var _response = await _apicall.Post<ResetPasswordResponseModel>(_regjson);
                                if (_response != null)
                                {
                                    if (response.message.Equals("Message Sent"))
                                    {
                                        //Cache.GlobalEmail = string.Empty;
                                        UserDialogs.Instance.Loading().Hide();
                                        //await Application.Current.MainPage.DisplayAlert(_response.message, _response.data, "OK");
                                        await _navigation.PushAsync(new MainMasterDetailPage());
                                        return;
                                    }
                                    else
                                    {                                       
                                        UserDialogs.Instance.Loading().Hide();
                                        //await Application.Current.MainPage.DisplayAlert(_response.message, _response.data, "OK");
                                        await _navigation.PushAsync(new MainMasterDetailPage());
                                        return;
                                    }
                                }                               
                                UserDialogs.Instance.HideLoading();
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

        private async void BindPickers()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    ProductName = Cache.globalProduct;
                    SelectedCatagory = Cache.globalCatagory;

                    IsComplaintDateEntryVisible = false;
                    IsComplaintDatePickerVisible = true;

                    List<string> catagory = new List<string> { "Electronics", "Electrical", "IT", "Daily Services" };
                    Catagory = catagory;

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

        private async Task<bool> IsRegistrationValidation()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                await Application.Current.MainPage.DisplayAlert("Book Service Error", "Product name required.", "OK");
                return false;
            }            
            else if(string.IsNullOrWhiteSpace(SelectedCatagory))
            {
                await Application.Current.MainPage.DisplayAlert("Book Service Error", "Select catagory field.", "OK");
                return false;
            }            
            else if (string.IsNullOrWhiteSpace(Complaint))
            {
                await Application.Current.MainPage.DisplayAlert("Book Service Error", "Complaint required.", "OK");
                return false;
            }

            return true;
        }
        #endregion
    }
}
