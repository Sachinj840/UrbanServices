using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class SubBannerOneUploadPageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        int SlidePosition = 0;
        private string _profileImage = String.Empty;
        private IMedia _mediaPicker;
        private string filePath;
        #endregion

        #region Constructor
        public SubBannerOneUploadPageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Sub Banner Upload";
            IsMenuVisible = false;
            IsBackButtonVisible = true;
            DeleteCommand = new DelegateCommand(ExecuteOnDelete);
            UploadPhotoCommand = new DelegateCommand(ExecuteUploadPhoto);
            BannerImageList = new ObservableCollection<BannerImages>();
            BannerImage = new ReadOnlyObservableCollection<BannerImages>(BannerImageList);
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

        private ReadOnlyObservableCollection<BannerImages> bannerImage;
        public ReadOnlyObservableCollection<BannerImages> BannerImage
        {
            get { return bannerImage; }
            set { bannerImage = value; OnPropertyChanged(); }
        }

        private ObservableCollection<BannerImages> _bannerImageList;

        public ObservableCollection<BannerImages> BannerImageList
        {
            get { return _bannerImageList; }
            set { _bannerImageList = value; OnPropertyChanged(); }
        }

        private ImageSource _preUserProfileImage;
        public ImageSource PreUserProfileImage
        {
            get { return _preUserProfileImage; }
            set { _preUserProfileImage = value; OnPropertyChanged(); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; OnPropertyChanged(); }
        }
        
        #endregion

        #region Delegate Commonds                     
        public DelegateCommand UploadPhotoCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        #endregion

        #region Methods                    
        private async void ExecuteUploadPhoto(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    var isValidate = await IsValidation();

                    if (isValidate)
                    {
                        var action = await Application.Current.MainPage.DisplayActionSheet("Upload Banner Image", "Cancel", null, "Select Photo");
                        if (action == "Select Photo")
                            await SelectPicture();
                        //else if (action == "Take Photo")
                        //    await TakePicture();
                        else if (action == "Cancel")
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
            }

        }

        private async void ExecuteOnDelete(object parma)
        {
            var result = await UserDialogs.Instance.ConfirmAsync("Are you sure want to delete this banner image?", "Delete Banner Image?", "YES", "NO");
            if (result)
            {
                HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_DeleteBannerImages, parma, false), Settings.AccessTokenSettings);
                var response = await apicall.GetResponse<RegistrationResponseModel>();
                if (response != null)
                {
                    if (response.regStatus == "successful")
                    {
                        UserDialogs.Instance.Loading().Hide();
                        await Application.Current.MainPage.DisplayAlert("Banner Image", response.message, "OK");
                        Cache.goToBackButtonText = "MainAdminHomePage";
                        UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                        await _navigation.PushAsync(new SubBannerOneUploadPage());
                    }
                    else
                    {
                        UserDialogs.Instance.Loading().Hide();
                        await Application.Current.MainPage.DisplayAlert("Banner Image", response.message, "OK");
                        return;
                    }
                }
                UserDialogs.Instance.Loading().Hide();
            }
        }
        private async void LoadListData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                bool isCallFirstTime = false;
                BannerImages bannerModel = new BannerImages();
                HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetbannerImages, false), Settings.AccessTokenSettings);
                var response = await apicall.GetResponse<BannerResponse>();
                if (response != null)
                {
                    if (response.subOneBanner != null)
                    {
                        if (response.subOneBanner.Count > 0)
                        {
                            IsStatusVisible = false;
                            int count = 0;
                            foreach (var item in response.subOneBanner.ToList())
                            {
                                count++;
                                string filepath = AppSetting.Root_Url + item.ImageData;
                                BannerImages _bannerImg = new BannerImages();
                                _bannerImg.ImageId = item.ImageId;
                                _bannerImg.Image = filepath;
                                _bannerImg.srNo = count;
                                _bannerImg.description = item.Description;
                                BannerImageList.Add(_bannerImg);
                            }

                            BannerImage = new ReadOnlyObservableCollection<BannerImages>(BannerImageList);
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
                            IsStatusVisible = true;
                        }
                    }
                    else
                    {
                        IsStatusVisible = true;
                    }
                }
                else
                {
                    IsStatusVisible = true;
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

        private async Task<bool> IsValidation()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                await Application.Current.MainPage.DisplayAlert("Upload Error", "Description required.", "OK");
                return false;
            }            
            return true;
        }
        #endregion

        #region Photos
        private async void Setup()
        {
            if (_mediaPicker != null)
            {
                return;
            }

            await CrossMedia.Current.Initialize();
            _mediaPicker = CrossMedia.Current;
        }
        private async Task SelectPicture()
        {
            Setup();

            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    BannerModel bannerData = new BannerModel();
                    var mediaFile = await this._mediaPicker.PickPhotoAsync();
                    //var mediaFile = await DependencyService.Get<ICameraService>().PickFileAsync();
                    if (mediaFile == null)
                    {
                        return;
                    }
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);

                    var memoryStream = new MemoryStream();
                    await mediaFile.GetStream().CopyToAsync(memoryStream);
                    var imageAsByte = memoryStream.ToArray();
                    bannerData.imageData = Convert.ToBase64String(imageAsByte);
                    bannerData.description = Description;
                    bannerData.IsSubOneTrue = true;

                    //add code to save image in db                   
                    HttpClientHelper apicall = new HttpClientHelper(ApiUrls.Url_SaveSubBannerImages, Settings.AccessTokenSettings);

                    var regjson = JsonConvert.SerializeObject(bannerData);
                    var response = await apicall.Post<CommanResponse>(regjson);

                    if (response != null)
                    {
                        if (response.regStatus == "successful")
                        {
                            await Application.Current.MainPage.DisplayAlert("Upload Banner", response.message, "OK");
                            // UserDialogs.Instance.HideLoading();
                            //LoadListData();
                            await _navigation.PushAsync(new SubBannerOneUploadPage());
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await Application.Current.MainPage.DisplayAlert("Upload Banner Error", response.message, "OK");
                            return;
                        }
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await Application.Current.MainPage.DisplayAlert("Upload Banner Error", response.message, "OK");
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
            }

        }
        private async Task TakePicture()
        {
            Setup();
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

                //var mediaFile = await this._mediaPicker.TakePhotoAsync(new StoreCameraMediaOptions
                //{
                //    DefaultCamera = CameraDevice.Front
                //});

                if (photo == null)
                {
                    return;
                }

                var memoryStream = new MemoryStream();
                await photo.GetStream().CopyToAsync(memoryStream);
                byte[] imageAsByte = memoryStream.ToArray();
                _profileImage = Convert.ToBase64String(imageAsByte);
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
        }
        private static byte[] ReadStreamByte(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion
    }
}
