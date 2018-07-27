using Acr.UserDialogs;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.ResponseModels;
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
     public  class NotificationPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private string _profileImage = String.Empty, _type= string.Empty;
        private bool isAdmin = false;
        #endregion

        #region Constructor
        public NotificationPageViewModel(INavigation navigation,string type) : base(navigation)
        {
            _type = type;
            _navigation = navigation;
            PageHeaderText = "Notifications";           
            IsMenuVisible = false;
            IsBackButtonVisible = true;
            NotificationList = new ObservableCollection<AdminNotificationModel>();
            NotificationData = new ReadOnlyObservableCollection<AdminNotificationModel>(NotificationList);
            LoadListData();
        }
        #endregion

        #region Properties         

        private ObservableCollection<AdminNotificationModel> notificationList;

        public ObservableCollection<AdminNotificationModel> NotificationList
        {
            get { return notificationList; }
            set { notificationList = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<AdminNotificationModel> notificationData;
        public ReadOnlyObservableCollection<AdminNotificationModel> NotificationData
        {
            get { return notificationData; }
            set { notificationData = value; OnPropertyChanged(); }
        }

        private bool _isStatusVisible;
        public bool IsStatusVisible
        {
            get { return _isStatusVisible; }
            set { _isStatusVisible = value; OnPropertyChanged(); }
        }
       

        #endregion

        #region Delegate Commonds                     

        #endregion

        #region Methods                    
        private async void LoadListData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                HttpClientHelper apicall = null;
                if (_type == "User")
                {
                    apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetUserNotificationData), Settings.AccessTokenSettings);
                }
                else
                {
                    isAdmin = true;
                    apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetAdminNotificationData), Settings.AccessTokenSettings);
                }
                
                var response = await apicall.GetResponse<List<AdminNotificationResponseModel>>();
                if (response != null)
                {
                    if(response.Count > 0)
                    {
                        IsStatusVisible = false;
                        foreach (var item in response.ToList())
                        {
                            AdminNotificationModel notificationModel = new AdminNotificationModel();
                            if (isAdmin == false) {
                                notificationModel.NotificationInfo = "Admin approved your complaint for service " + item.productName + " in " + item.catagory + " catagory";
                            }
                            else
                            {
                                notificationModel.NotificationInfo = item.userName + " registered complaint for service " + item.productName + " in " + item.catagory + " catagory";
                            }                                                       
                            NotificationList.Add(notificationModel);
                        }
                    }
                    else
                    {
                        IsStatusVisible = true;
                    }
                    NotificationData = new ReadOnlyObservableCollection<AdminNotificationModel>(NotificationList);
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

        #endregion
    }
}
