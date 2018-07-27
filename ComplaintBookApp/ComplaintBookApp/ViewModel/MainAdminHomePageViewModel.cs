using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Views;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class MainAdminHomePageViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        int SlidePosition = 0;
        private string _profileImage = String.Empty;
        #endregion

        #region Constructor
        public MainAdminHomePageViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Admin Home";
            IsMenuVisible = true;           
            IsBackButtonVisible = false;
            SubBannerOneUploadCommand = new DelegateCommand(ExecuteOnSubOneBannerUpload);
            SubBannerTwoUploadCommand = new DelegateCommand(ExecuteOnSubTwoBannerUpload);
            MainBannerUploadCommand = new DelegateCommand(ExecuteOnMainBannerUpload);
            ApproveServiceCommand = new DelegateCommand(ExecuteOnApproveService);
            UserInfoCommand = new DelegateCommand(ExecuteOnUserInfoCommand);
        }
        #endregion

        #region Properties                                   

        private ImageSource _preUserProfileImage;
        public ImageSource PreUserProfileImage
        {
            get { return _preUserProfileImage; }
            set { _preUserProfileImage = value; OnPropertyChanged(); }
        }

        #endregion

        #region Delegate Commonds   
        public DelegateCommand UserInfoCommand { get; set; }
        public DelegateCommand MainBannerUploadCommand { get; set; }
        public DelegateCommand SubBannerOneUploadCommand { get; set; }
        public DelegateCommand SubBannerTwoUploadCommand { get; set; }
        public DelegateCommand ApproveServiceCommand { get; set; }
        public ICommand IconPressedCommand
        {
            get
            {
                return new Command<string>(OnClickIcon);
            }
        }
        #endregion

        #region Methods                
        private async void ExecuteOnUserInfoCommand(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "MainAdminHomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new UserInfoListPage());                    
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
        private async void ExecuteOnSubOneBannerUpload(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "MainAdminHomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new SubBannerOneUploadPage());                    
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

        private async void ExecuteOnSubTwoBannerUpload(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "MainAdminHomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new SubBannerTwoUploadPage());
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

        private async void ExecuteOnMainBannerUpload(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "MainAdminHomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new BannerUploadPage());
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
        
        private async void ExecuteOnApproveService(object parma)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "MainAdminHomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new ApproveServiceComplaintListPage());
                   // UserDialogs.Instance.HideLoading();
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

                    Cache.goToBackButtonText = "MainAdminHomePage";
                    var pageType = (ApplicationActivity)Enum.Parse(typeof(ApplicationActivity), param);
                    await PageNavigation(ApplicationActivity.CheckApprovedServiceStatusListPage, pageType);
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
