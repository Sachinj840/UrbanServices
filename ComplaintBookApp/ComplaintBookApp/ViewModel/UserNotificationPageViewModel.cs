using Acr.UserDialogs;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model.ResponseModels;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class UserNotificationPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public UserNotificationPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Notification";
            IsBackButtonVisible = true;
            IsMenuVisible = false;
            BindData();
        }
        #endregion

        #region Property

        private int _position;
        public int Position { get { return _position; } set { _position = value; OnPropertyChanged(); } }

        #endregion

        #region Delegate Commonds        
        #endregion

        #region Methods     
        private async void BindData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {                    
                    //UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);

                    //add code to get dropdown list table data               
                    HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetDropDownListData), String.Empty);
                    var response = await apicall.GetResponse<ListResponseModel>();
                    if (response != null)
                    {
                       
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
        #endregion
    }
}
