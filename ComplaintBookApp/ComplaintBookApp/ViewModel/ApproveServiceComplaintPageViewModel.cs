using Acr.UserDialogs;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Views;
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
    public class ApproveServiceComplaintPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;        
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public ApproveServiceComplaintPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Complaint List";
            PageSubHeaderText = "Pending";
            IsMenuVisible = false;
            IsBackButtonVisible = true;
            ServiceComplaintList = new ObservableCollection<ApproveServiceModel>();
            ServiceComplaintData = new ReadOnlyObservableCollection<ApproveServiceModel>(ServiceComplaintList);
            LoadListData();
        }
        #endregion

        #region Properties                                           

        private bool _isStatusVisible;
        public bool IsStatusVisible
        {
            get { return _isStatusVisible; }
            set { _isStatusVisible = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<ApproveServiceModel> serviceComplaintData;
        public ReadOnlyObservableCollection<ApproveServiceModel> ServiceComplaintData
        {
            get { return serviceComplaintData; }
            set { serviceComplaintData = value; OnPropertyChanged(); }
        }

        private ObservableCollection<ApproveServiceModel> serviceComplaintList;
        public ObservableCollection<ApproveServiceModel> ServiceComplaintList
        {
            get { return serviceComplaintList; }
            set { serviceComplaintList = value; OnPropertyChanged(); }
        }

        #endregion

        #region Delegate Commonds                     
        public ICommand TapCommand
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
                    Cache.goToBackButtonText = "ApproveComplaintPage";
                    await _navigation.PushAsync(new ApproveServiceComplaintDetailPage(data, ApplicationActivity.Pending));

                });
            }
        }
        #endregion

        #region Methods                    
        private async void LoadListData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                ApproveServiceModel approveModel = new ApproveServiceModel();
                HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPendingServiceList), Settings.AccessTokenSettings);
                var response = await apicall.GetResponse<List<ApproveServiceModel>>();
                if (response != null)
                {
                    if (response.Count > 0)
                    {
                        IsStatusVisible = false;
                        foreach (var item in response)
                        {
                            ApproveServiceModel approveData = new ApproveServiceModel();
                            approveData.Catagory = item.Catagory;
                            approveData.ComplaintDate = item.ComplaintDate;
                            approveData.complaintId = item.complaintId;
                            approveData.ComplaintSummary = item.ComplaintSummary;
                            approveData.Date_Complaint = item.Date_Complaint;
                            approveData.ProductName = item.ProductName;
                            approveData.status = item.status;
                            approveData.Title = item.Title;
                            ServiceComplaintList.Add(approveData);
                        }
                        ServiceComplaintData = new ReadOnlyObservableCollection<ApproveServiceModel>(ServiceComplaintList);
                    }
                    else
                    {
                        IsStatusVisible = true;
                        UserDialogs.Instance.HideLoading();
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

        #endregion
    }
}
