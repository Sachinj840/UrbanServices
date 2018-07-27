using Acr.UserDialogs;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
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
     public class CheckServiceStatusListPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public CheckServiceStatusListPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Complaint List";
            IsMenuVisible = false;
            IsBackButtonVisible = true;
            ServiceComplaintList = new ObservableCollection<ApproveServiceModel>();
            LoadListData();
        }
        #endregion

        #region Properties                                           

        private ObservableCollection<ApproveServiceModel> serviceComplaintList;
        public ObservableCollection<ApproveServiceModel> ServiceComplaintList
        {
            get { return serviceComplaintList; }
            set { serviceComplaintList = value; OnPropertyChanged(); }
        }

        #endregion

        #region Delegate Commonds                     

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
                        ServiceComplaintList = new ObservableCollection<ApproveServiceModel>(response);
                    }
                    else
                    {
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
