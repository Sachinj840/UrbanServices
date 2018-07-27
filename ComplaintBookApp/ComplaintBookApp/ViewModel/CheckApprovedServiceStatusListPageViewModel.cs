using Acr.UserDialogs;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class CheckApprovedServiceStatusListPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public CheckApprovedServiceStatusListPageViewModel(INavigation navigation, ApplicationActivity pageType) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Complaint List";
            PageSubHeaderText = pageType == ApplicationActivity.CheckApprovedServiceStatusListPage ? "Approved" : "InProgress";
            IsMenuVisible = false;
            IsBackButtonVisible = true;
            ServiceComplaintList = new ObservableCollection<ApproveServiceModel>();
            LoadListData(pageType);
        }
        #endregion

        #region Properties         
        
        private bool _isStatusVisible;
        public bool IsStatusVisible
        {
            get { return _isStatusVisible; }
            set { _isStatusVisible = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ApproveServiceModel> serviceComplaintList;
        public ObservableCollection<ApproveServiceModel> ServiceComplaintList
        {
            get { return serviceComplaintList; }
            set { serviceComplaintList = value; OnPropertyChanged(); }
        }

        #endregion

        #region Delegate Commonds                     
        public ICommand ProductTapCommand
        {
            get
            {
                return new Command<ApproveServiceModel>(async data =>
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    if (data == null)
                    {
                        return;
                    }
                    if(data.status == "Approved")
                    {
                        Cache.goToBackButtonText = "StatusComplaintPage";
                        await _navigation.PushAsync(new ApproveServiceComplaintDetailPage(data, ApplicationActivity.CheckApprovedServiceStatusListPage));
                    }
                    else
                    {
                        Cache.goToBackButtonText = "StatusComplaintPage";
                        await _navigation.PushAsync(new ApproveServiceComplaintDetailPage(data, ApplicationActivity.CheckInprogressServiceStatusListPage));
                    }

                });
            }
        }
        #endregion

        #region Methods                    
        private async void LoadListData(ApplicationActivity pageType)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                bool isCallFirstTime = false;
                ReqProductModel productType = new ReqProductModel();
                ApproveServiceModel approveModel = new ApproveServiceModel();
                HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetApprovedInProgressServiceList, Settings.AccessTokenSettings);
                productType.productType = pageType;

                var regjson = JsonConvert.SerializeObject(productType);
                var response = await apicall.Post<List<ApproveServiceModel>>(regjson);

                switch (pageType)
                {
                    case ApplicationActivity.CheckApprovedServiceStatusListPage:
                        ApproveBackgroundColor = Color.FromHex("#981C1C");
                        InProgressBackgroundColor = Color.FromHex("#000000");
                        if (response != null)
                        {
                            if (response.Count > 0)
                            {
                                IsStatusVisible = false;
                                ServiceComplaintList = new ObservableCollection<ApproveServiceModel>(response);
                            }
                            else
                            {
                                IsStatusVisible = true;
                                UserDialogs.Instance.HideLoading();
                            }
                        }
                        break;
                    case ApplicationActivity.CheckInprogressServiceStatusListPage:
                        ApproveBackgroundColor = Color.FromHex("#000000");
                        InProgressBackgroundColor = Color.FromHex("#981C1C");
                        if (response != null)
                        {
                            if (response.Count > 0)
                            {
                                IsStatusVisible = false;
                                ServiceComplaintList = new ObservableCollection<ApproveServiceModel>(response);
                            }
                            else
                            {
                                IsStatusVisible = true;
                                UserDialogs.Instance.HideLoading();
                            }
                        }
                        break;
                    default:
                        break;
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

        #endregion
    }
}
