using Acr.UserDialogs;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
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
    public class ServiceCommentListPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private string filePath;
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public ServiceCommentListPageViewModel(INavigation navigation) : base(navigation)
        {
            LoadServiceCommentListData();
            _navigation = navigation;
            ServiceCommentList = new ObservableCollection<ServiceCommentModel>();
            ServiceCommentData = new ReadOnlyObservableCollection<ServiceCommentModel>(ServiceCommentList);
            PageHeaderText = "Service Comment List";
            IsMenuVisible = false;
            IsBackButtonVisible = true;
        }
        #endregion

        #region Properties                            

        private ObservableCollection<ServiceCommentModel> serviceCommentList;
        public ObservableCollection<ServiceCommentModel> ServiceCommentList
        {
            get { return serviceCommentList; }
            set { serviceCommentList = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<ServiceCommentModel> serviceCommentData;
        public ReadOnlyObservableCollection<ServiceCommentModel> ServiceCommentData
        {
            get { return serviceCommentData; }
            set { serviceCommentData = value; OnPropertyChanged(); }
        }        

        #endregion

        #region Delegate Commonds        

        public ICommand ProductTapCommand
        {
            get
            {
                return new Command<ProductModel>(async products =>
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    if (products == null)
                    {
                        return;
                    }                    
                    Cache.goToBackButtonText = "MainHomePage";
                   // await _navigation.PushAsync(new BookServiceComplaintPage());
                    UserDialogs.Instance.HideLoading();
                });
            }
        }
        #endregion

        #region Methods   
        private async void LoadServiceCommentListData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    //ReqProductModel productType = new ReqProductModel();
                    //HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetServiceCommentData, Settings.AccessTokenSettings);                

                    //var regjson = JsonConvert.SerializeObject(productType);
                    //var response = await apicall.Post<ProductListResponseModel>(regjson);

                    //if (response != null)
                    //{
                    //    foreach (var item in response.electrical)
                    //    {
                    //        byte[] imageByte = null;
                    //        imageByte = Convert.FromBase64String(item.serviceImage);
                    //        ServiceCommentModel serviceCommentModel = new ServiceCommentModel();
                    //        serviceCommentModel.ServiceCommentName = item.serviceName;
                    //        ServiceCommentList.Add(serviceCommentModel);
                    //    }
                    //    ServiceCommentData = new ReadOnlyObservableCollection<ServiceCommentModel>(ServiceCommentList);
                    //    UserDialogs.Instance.HideLoading();
                    //}
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Application.Current.MainPage.DisplayAlert("Network", AppConstant.NETWORK_FAILURE, "OK");
                    return;
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
     
        #endregion
    }
}
