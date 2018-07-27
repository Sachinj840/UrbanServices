using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.RequestModels;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Views;
using ComplaintBookApp.Views.MasterDetailPages;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class ProductListPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private string filePath;
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public ProductListPageViewModel(INavigation navigation, ApplicationActivity pageType) : base(navigation)
        {           
            LoadProductListData(pageType);
            _navigation = navigation;
            ProductList = new ObservableCollection<ProductModel>();
            ProductData = new ReadOnlyObservableCollection<ProductModel>(ProductList);            
            PageHeaderText = "Services";
            IsMenuVisible = false;
            IsBackButtonVisible = true;
        }
        #endregion

        #region Properties                            

        private ObservableCollection<ProductModel> productList;

        public ObservableCollection<ProductModel> ProductList
        {
            get { return productList; }
            set { productList = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<ProductModel> productData;
        public ReadOnlyObservableCollection<ProductModel> ProductData
        {
            get { return productData; }
            set { productData = value; OnPropertyChanged(); }
        }


        private ImageSource _recUserProfileImage;
        public ImageSource RecUserProfileImage
        {
            get { return _recUserProfileImage; }
            set { _recUserProfileImage = value; OnPropertyChanged(); }
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
                    Cache.globalProduct = products.ProductName;
                    Cache.goToBackButtonText = "MainHomePage";
                    await _navigation.PushAsync(new BookServiceComplaintPage());
                    UserDialogs.Instance.HideLoading();
                });
            }
        }
        #endregion

        #region Methods   
        private async void LoadProductListData(ApplicationActivity pageType)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    bool isCallFirstTime = false;
                    ReqProductModel productType = new ReqProductModel();
                    HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_GetProductListData, Settings.AccessTokenSettings);
                    productType.productType = pageType;

                    var regjson = JsonConvert.SerializeObject(productType);
                    var response = await apicall.Post<ProductListResponseModel>(regjson);

                    if (response != null)
                    {                       
                        switch (pageType)
                        {
                            case ApplicationActivity.Electrical:
                                if (response.electrical != null)
                                {
                                    Cache.globalCatagory = "Electrical";
                                    PageSubHeaderText = "Electrical";
                                    foreach (var item in response.electrical)
                                    {
                                        string filepath = AppSetting.Root_Url + item.serviceImage;
                                        //byte[] imageByte = null;
                                        //imageByte = Convert.FromBase64String(item.serviceImage);
                                        ProductModel productModel = new ProductModel();
                                        productModel.ProductName = item.serviceName;
                                        productModel.ProductImage = filepath; //ImageSource.FromUri(new Uri(filepath));  //ImageSource.FromStream(() => new MemoryStream(imageByte));
                                        ProductList.Add(productModel);
                                    }
                                }
                                break;
                            case ApplicationActivity.Electronic:
                                if (response.electronic != null)
                                {
                                    Cache.globalCatagory = "Electronics";
                                    PageSubHeaderText = "Electronics";
                                    foreach (var item in response.electronic)
                                    {
                                        string filepath = AppSetting.Root_Url + item.serviceImage;
                                        //byte[] imageByte = null;
                                        //imageByte = Convert.FromBase64String(item.serviceImage);
                                        ProductModel productModel = new ProductModel();
                                        productModel.ProductName = item.serviceName;
                                        productModel.ProductImage = filepath; //ImageSource.FromUri(new Uri(filepath));  //ImageSource.FromStream(() => new MemoryStream(imageByte));
                                        ProductList.Add(productModel);
                                    }
                                }
                                break;
                            case ApplicationActivity.IT:
                                if (response.it != null)
                                {
                                    Cache.globalCatagory = "IT";
                                    PageSubHeaderText = "IT";
                                    foreach (var item in response.it)
                                    {
                                        string filepath = AppSetting.Root_Url + item.serviceImage;
                                        //byte[] imageByte = null;
                                        //imageByte = Convert.FromBase64String(item.serviceImage);
                                        ProductModel productModel = new ProductModel();
                                        productModel.ProductName = item.serviceName;
                                        productModel.ProductImage = filepath; //ImageSource.FromUri(new Uri(filepath));  //ImageSource.FromStream(() => new MemoryStream(imageByte));
                                        ProductList.Add(productModel);
                                    }
                                }
                                break;
                            case ApplicationActivity.DailyNeeds:
                                if (response.dailyNeeds != null)
                                {
                                    Cache.globalCatagory = "Daily Services";
                                    PageSubHeaderText = "Daily Services";
                                    foreach (var item in response.dailyNeeds)
                                    {
                                        string filepath = AppSetting.Root_Url + item.serviceImage;
                                        //byte[] imageByte = null;
                                        //imageByte = Convert.FromBase64String(item.serviceImage);
                                        ProductModel productModel = new ProductModel();
                                        productModel.ProductName = item.serviceName;
                                        productModel.ProductImage = filepath; //ImageSource.FromUri(new Uri(filepath));  //ImageSource.FromStream(() => new MemoryStream(imageByte));
                                        ProductList.Add(productModel);
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    ProductData = new ReadOnlyObservableCollection<ProductModel>(ProductList);
                    if (Cache.globalCatagory == "Electronics")
                    {
                        Device.StartTimer(TimeSpan.FromSeconds(10), () =>
                        {
                            if (isCallFirstTime == false)
                            {
                                isCallFirstTime = true;
                                UserDialogs.Instance.HideLoading();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                    else
                    {
                        Device.StartTimer(TimeSpan.FromSeconds(4), () =>
                        {
                            if (isCallFirstTime == false)
                            {
                                isCallFirstTime = true;
                                UserDialogs.Instance.HideLoading();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                    
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
