using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.ViewModel.MasterDetailViewModel;
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
    public class MainHomePageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        int MainSlidePosition = 0, SlidePosition= 0;
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public MainHomePageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            UpdateMenuList();
            BannerImageHeight = Application.Current.MainPage != null ? (Application.Current.MainPage.Height) / 3 : 500;
            SubOneBannerImagesList = new ObservableCollection<SubBannerOneImages>();
            SubOneBannerImgs = new ReadOnlyObservableCollection<SubBannerOneImages>(SubOneBannerImagesList);
            SubTwoBannerImagesList = new ObservableCollection<SubBannerTwoImages>();
            SubTwoBannerImgs = new ReadOnlyObservableCollection<SubBannerTwoImages>(SubTwoBannerImagesList);
            MainBannerImagesList = new ObservableCollection<BannerImages>();
            MainBannerImgs = new ReadOnlyObservableCollection<BannerImages>(MainBannerImagesList);
            PageHeaderText = "Urban Services";
            IsMenuVisible = true;
            IsSkipVisible = false;
            IsBackButtonVisible = false;
            LoadBannerData();            
        }
        #endregion

        #region Properties                                   

        private double bannerimageheight;
        public double BannerImageHeight
        {
            get { return bannerimageheight; }
            set { bannerimageheight = value; OnPropertyChanged(); }
        }

        private int _mainposition;
        public int MainPosition { get { return _mainposition; } set { _mainposition = value; OnPropertyChanged(); } }

        private ObservableCollection<BannerImages> mainBannerImagesList;
        public ObservableCollection<BannerImages> MainBannerImagesList
        {
            get { return mainBannerImagesList; }
            set { mainBannerImagesList = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<BannerImages> _mainbannerImgs;
        public ReadOnlyObservableCollection<BannerImages> MainBannerImgs
        {
            get { return _mainbannerImgs; }
            set { _mainbannerImgs = value; OnPropertyChanged(); }
        }

        private int _position;
        public int Position { get { return _position; } set { _position = value; OnPropertyChanged(); } }

        private ObservableCollection<SubBannerOneImages> subOneBannerImagesList;
        public ObservableCollection<SubBannerOneImages> SubOneBannerImagesList
        {
            get { return subOneBannerImagesList; }
            set { subOneBannerImagesList = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<SubBannerTwoImages> _subTwobannerImgs;
        public ReadOnlyObservableCollection<SubBannerTwoImages> SubTwoBannerImgs
        {
            get { return _subTwobannerImgs; }
            set { _subTwobannerImgs = value; OnPropertyChanged(); }
        }

        private ObservableCollection<SubBannerTwoImages> subTwoBannerImagesList;
        public ObservableCollection<SubBannerTwoImages> SubTwoBannerImagesList
        {
            get { return subTwoBannerImagesList; }
            set { subTwoBannerImagesList = value; OnPropertyChanged(); }
        }

        private ReadOnlyObservableCollection<SubBannerOneImages> _subOnebannerImgs;
        public ReadOnlyObservableCollection<SubBannerOneImages> SubOneBannerImgs
        {
            get { return _subOnebannerImgs; }
            set { _subOnebannerImgs = value; OnPropertyChanged(); }
        }        

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private ImageSource _userProfileImage;
        public ImageSource UserProfileImage
        {
            get { return _userProfileImage; }
            set { _userProfileImage = value; OnPropertyChanged(); }
        }       

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged(); }
        }        

        #endregion

        #region Delegate Commonds                     
        public ICommand IconPressedCommand
        {
            get
            {
                return new Command<string>(OnClickIcon);
            }
        }
        #endregion

        #region Methods                    
        private async void LoadBannerData()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    int count = 0, subonecount = 0 , subtwocount = 0;

                    //get db data to load in mainbanner
                    HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetbannerImages,true), Settings.AccessTokenSettings);
                    var response = await apicall.GetResponse<BannerResponse>();
                    if (response != null)
                    {
                        if (response.banner != null)
                        {
                            if (response.banner.Count > 0)
                            {
                                foreach (var item in response.banner.ToList())
                                {
                                    count++;
                                    if (count < 7)
                                    {
                                        string filepath = AppSetting.Root_Url + item.ImageData;
                                        BannerImages _bannerImg = new BannerImages();
                                        _bannerImg.ImageId = item.ImageId;
                                        _bannerImg.Image = filepath;
                                        //var imageBytes = Convert.FromBase64String(item.ImageData);
                                        //_bannerImg.Image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                                        MainBannerImagesList.Add(_bannerImg);
                                    }
                                }
                            }
                            else
                            {
                                BannerImages _bannerImg = new BannerImages();
                                string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                                _bannerImg.Image = imagePath;// "banner.png";
                                MainBannerImagesList.Add(_bannerImg);
                            }
                        }
                        else
                        {
                            BannerImages _bannerImg = new BannerImages();
                            string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                            _bannerImg.Image = imagePath;// "banner.png";
                            MainBannerImagesList.Add(_bannerImg);
                        }
                    }
                    else
                    {
                        BannerImages _bannerImg = new BannerImages();
                        string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                        _bannerImg.Image = imagePath;// "banner.png";
                        MainBannerImagesList.Add(_bannerImg);
                    }
                    
                    MainBannerImgs = new ReadOnlyObservableCollection<BannerImages>(MainBannerImagesList);
                    //UserDialogs.Instance.HideLoading();                    

                    //slide show images
                    Device.StartTimer(TimeSpan.FromSeconds(6), () =>
                    {
                        MainSlidePosition++;
                        if (MainSlidePosition == MainBannerImgs.Count) MainSlidePosition = 0;
                        MainPosition = MainSlidePosition;
                        return true;
                    });

                    //load sub banner one data

                    //load sub banner one data

                    SubBannerOneImages banner_Img = new SubBannerOneImages();
                    string image_Path = AppSetting.Root_Url + "Portals/0/UrbanService/Images/AC.png";
                    banner_Img.SubOneImage = image_Path;
                    banner_Img.SubOneImagelabel = "AC";
                    banner_Img.Catagory = "Electrical";
                    SubOneBannerImagesList.Add(banner_Img);

                    SubBannerOneImages banner_Img1 = new SubBannerOneImages();
                    string image_Path1 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Fridge.png";
                    banner_Img1.SubOneImage = image_Path1;
                    banner_Img1.SubOneImagelabel = "Fridge";
                    banner_Img1.Catagory = "Electronics";
                    SubOneBannerImagesList.Add(banner_Img1);

                    SubBannerOneImages banner_Img2 = new SubBannerOneImages();
                    string image_Path2 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/TV.jpg";
                    banner_Img2.SubOneImage = image_Path2;
                    banner_Img2.SubOneImagelabel = "TV ( LCD / LED )";
                    banner_Img2.Catagory = "Electronics";
                    SubOneBannerImagesList.Add(banner_Img2);

                    SubBannerOneImages banner_Img3 = new SubBannerOneImages();
                    string image_Path3 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Computer.png";
                    banner_Img3.SubOneImage = image_Path3;
                    banner_Img3.SubOneImagelabel = "Computer / Laptop";
                    banner_Img3.Catagory = "Electronics";
                    SubOneBannerImagesList.Add(banner_Img3);

                    SubBannerOneImages banner_Img4 = new SubBannerOneImages();
                    string image_Path4 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Washing_Machine.png";
                    banner_Img4.SubOneImage = image_Path4;
                    banner_Img4.SubOneImagelabel = "Washing Machine";
                    banner_Img4.Catagory = "Electronics";
                    SubOneBannerImagesList.Add(banner_Img4);

                    SubBannerOneImages banner_Img5 = new SubBannerOneImages();
                    string image_Path5 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Geyser.png";
                    banner_Img5.SubOneImage = image_Path5;
                    banner_Img5.SubOneImagelabel = "Geyser";
                    banner_Img5.Catagory = "Electrical";
                    SubOneBannerImagesList.Add(banner_Img5);

                    SubBannerOneImages banner_Img6 = new SubBannerOneImages();
                    string image_Path6 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Printer.png";
                    banner_Img6.SubOneImage = image_Path6;
                    banner_Img6.SubOneImagelabel = "Printer";
                    banner_Img6.Catagory = "Electronics";
                    SubOneBannerImagesList.Add(banner_Img6);

                    //load sub banner two data

                    SubBannerTwoImages bannerTwoImg = new SubBannerTwoImages();
                    string imagePathTwo = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Electrician.png";
                    bannerTwoImg.SubTwoImage = imagePathTwo;// "banner.png";
                    bannerTwoImg.SubTwoImagelabel = "Electrician";
                    bannerTwoImg.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg);

                    SubBannerTwoImages bannerTwoImg1 = new SubBannerTwoImages();
                    string imagePathTwo1 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Plumber.png";
                    bannerTwoImg1.SubTwoImage = imagePathTwo1;// "banner.png";
                    bannerTwoImg1.SubTwoImagelabel = "Plumber";
                    bannerTwoImg1.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg1);

                    SubBannerTwoImages bannerTwoImg2 = new SubBannerTwoImages();
                    string imagePathTwo2 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Carpenter.png";
                    bannerTwoImg2.SubTwoImage = imagePathTwo2;// "banner.png";
                    bannerTwoImg2.SubTwoImagelabel = "Carpenter / Furnitur";
                    bannerTwoImg2.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg2);

                    SubBannerTwoImages bannerTwoImg3 = new SubBannerTwoImages();
                    string imagePathTwo3 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/CivilEnginner.jpg";
                    bannerTwoImg3.SubTwoImage = imagePathTwo3;// "banner.png";
                    bannerTwoImg3.SubTwoImagelabel = "Civil Engineer";
                    bannerTwoImg3.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg3);

                    SubBannerTwoImages bannerTwoImg4 = new SubBannerTwoImages();
                    string imagePathTwo4 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/PestControl.png";
                    bannerTwoImg4.SubTwoImage = imagePathTwo4;// "banner.png";
                    bannerTwoImg4.SubTwoImagelabel = "Pest Control";
                    bannerTwoImg4.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg4);

                    SubBannerTwoImages bannerTwoImg5 = new SubBannerTwoImages();
                    string imagePathTwo5 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/Painter.png";
                    bannerTwoImg5.SubTwoImage = imagePathTwo5;// "banner.png";
                    bannerTwoImg5.SubTwoImagelabel = "Painter";
                    bannerTwoImg5.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg5);

                    SubBannerTwoImages bannerTwoImg6 = new SubBannerTwoImages();
                    string imagePathTwo6 = AppSetting.Root_Url + "Portals/0/UrbanService/Images/fabricator.png";
                    bannerTwoImg6.SubTwoImage = imagePathTwo6;// "banner.png";
                    bannerTwoImg6.SubTwoImagelabel = "Fabricator";
                    bannerTwoImg6.Catagory = "Daily Services";
                    SubTwoBannerImagesList.Add(bannerTwoImg6);
                    UserDialogs.Instance.HideLoading();

                    ////get db data to load in subonebanner
                    //HttpClientHelper _apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetbannerImages, false), Settings.AccessTokenSettings);
                    //var _response = await _apicall.GetResponse<BannerResponse>();
                    //if (_response != null)
                    //{
                    //    if (_response.subOneBanner != null)
                    //    {
                    //        if (_response.subOneBanner.Count > 0)
                    //        {
                    //            foreach (var item in _response.subOneBanner.ToList())
                    //            {
                    //                subonecount++;
                    //                if (subonecount < 8)
                    //                {
                    //                    string filepath = AppSetting.Root_Url + item.ImageData;
                    //                    SubBannerOneImages _bannerImg = new SubBannerOneImages();
                    //                    _bannerImg.ImageId = item.ImageId;
                    //                    _bannerImg.SubOneImage = filepath;
                    //                    _bannerImg.SubOneImagelabel = string.IsNullOrEmpty(item.Description) ? "" : item.Description;
                    //                    //var imageBytes = Convert.FromBase64String(item.ImageData);
                    //                    //_bannerImg.Image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    //                    SubOneBannerImagesList.Add(_bannerImg);
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            SubBannerOneImages _bannerImg = new SubBannerOneImages();
                    //            string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //            _bannerImg.SubOneImage = imagePath;// "banner.png";
                    //            _bannerImg.SubOneImagelabel = "Banner Image";
                    //            SubOneBannerImagesList.Add(_bannerImg);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        SubBannerOneImages _bannerImg = new SubBannerOneImages();
                    //        string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //        _bannerImg.SubOneImage = imagePath;// "banner.png";
                    //        _bannerImg.SubOneImagelabel = "Banner Image";
                    //        SubOneBannerImagesList.Add(_bannerImg);
                    //    }
                    //    if (_response.subTwoBanner != null)
                    //    {
                    //        if (_response.subTwoBanner.Count > 0)
                    //        {
                    //            foreach (var item in _response.subTwoBanner.ToList())
                    //            {
                    //                subtwocount++;
                    //                if (subtwocount < 8)
                    //                {
                    //                    string filepath = AppSetting.Root_Url + item.ImageData;
                    //                SubBannerTwoImages _bannerImg = new SubBannerTwoImages();
                    //                _bannerImg.ImageId = item.ImageId;
                    //                _bannerImg.SubTwoImage = filepath;
                    //                _bannerImg.SubTwoImagelabel = string.IsNullOrEmpty(item.Description) ? "" : item.Description;
                    //                //var imageBytes = Convert.FromBase64String(item.ImageData);
                    //                //_bannerImg.Image = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    //                SubTwoBannerImagesList.Add(_bannerImg);
                    //                }
                    //            }
                    //        }
                    //        else
                    //        {
                    //            SubBannerTwoImages _bannerImg = new SubBannerTwoImages();
                    //            string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //            _bannerImg.SubTwoImage = imagePath;// "banner.png";
                    //            _bannerImg.SubTwoImagelabel = "Banner Image";
                    //            SubTwoBannerImagesList.Add(_bannerImg);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        SubBannerTwoImages _bannerImg = new SubBannerTwoImages();
                    //        string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //        _bannerImg.SubTwoImage = imagePath;// "banner.png";
                    //        _bannerImg.SubTwoImagelabel = "Banner Image";
                    //        SubTwoBannerImagesList.Add(_bannerImg);

                    //        SubBannerOneImages bannerImg = new SubBannerOneImages();
                    //        string _imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //        bannerImg.SubOneImage = _imagePath;// "banner.png";
                    //        bannerImg.SubOneImagelabel = "Banner Image";
                    //        SubOneBannerImagesList.Add(bannerImg);
                    //    }
                    //}
                    //else
                    //{
                    //    SubBannerTwoImages _bannerImg = new SubBannerTwoImages();
                    //    string imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //    _bannerImg.SubTwoImage = imagePath;// "banner.png";
                    //    _bannerImg.SubTwoImagelabel = "Banner Image";
                    //    SubTwoBannerImagesList.Add(_bannerImg);

                    //    SubBannerOneImages bannerImg = new SubBannerOneImages();
                    //    string _imagePath = AppSetting.Root_Url + "Portals/0/UrbanService/Images/banner.png";
                    //    bannerImg.SubOneImage = _imagePath;// "banner.png";
                    //    bannerImg.SubOneImagelabel = "Banner Image";
                    //    SubOneBannerImagesList.Add(bannerImg);
                    //}

                    //SubOneBannerImgs = new ReadOnlyObservableCollection<SubBannerOneImages>(SubOneBannerImagesList);
                    //SubTwoBannerImgs = new ReadOnlyObservableCollection<SubBannerTwoImages>(SubTwoBannerImagesList);
                    //UserDialogs.Instance.HideLoading();                                        
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
        private async void OnClickIcon(string param)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (string.IsNullOrEmpty(param))
                        return;

                    Cache.goToBackButtonText = "MainHomePage";
                    var pageType = (ApplicationActivity)Enum.Parse(typeof(ApplicationActivity), param);
                    await PageNavigation(ApplicationActivity.ProductListPage, pageType);
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
            }
        }

        private async void UpdateMenuList()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var masterDetailPage = Cache.MasterPage;
                    if (masterDetailPage == null)
                    {
                        masterDetailPage = Cache.MasterPage;
                    }

                    if (masterDetailPage == null)
                    {
                        return;
                    }

                    var masterPage = masterDetailPage.Master;
                    var bindingContext = masterPage.BindingContext as MainMasterDetailViewModel;
                    bindingContext.FillMenuItems();
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
            }
        }

        #endregion
    }
}
