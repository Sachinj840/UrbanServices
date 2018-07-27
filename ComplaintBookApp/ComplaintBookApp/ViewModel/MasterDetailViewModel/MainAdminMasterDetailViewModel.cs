using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Interfaces;
using ComplaintBookApp.Constants;
using ComplaintBookApp.Helpers;
using ComplaintBookApp.Model;
using ComplaintBookApp.Views;
using ComplaintBookApp.Views.MasterDetailPages;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel.MasterDetailViewModel
{
    public class MainAdminMasterDetailViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        private List<DrawerMenuItem> menuItems;
        #endregion

        #region Constructor
        public MainAdminMasterDetailViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            IsBackButtonVisible = true;
            TapBackCommand = new DelegateCommand(TapOnBackImage);
            FillMenuItems();
            menuItems = new List<DrawerMenuItem>();
        }
        #endregion

        #region Properties                            

        private DrawerMenuItem selectedmenuItem;
        public DrawerMenuItem SelectedMenuItem
        {
            get { return selectedmenuItem; }
            set { selectedmenuItem = value; OnPropertyChanged(); }
        }

        private bool isPresentedMenu;
        public bool IsPresentedMenu
        {
            get { return isPresentedMenu; }
            set { isPresentedMenu = value; OnPropertyChanged(); }
        }

        private List<DrawerMenuItem> _drawerMenuList;
        public List<DrawerMenuItem> DrawerMenuList
        {
            get { return _drawerMenuList; }
            set { _drawerMenuList = value; OnPropertyChanged(); }
        }

        private ImageSource _userProfileImage;
        public ImageSource UserProfileImage
        {
            get { return _userProfileImage; }
            set { _userProfileImage = value; OnPropertyChanged(); }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        #endregion

        #region Delegate Commonds
        public DelegateCommand TapBackCommand { get; set; }

        public ICommand MenuSelectedItemCommand
        {
            get
            {
                return new Command<DrawerMenuItem>(async item =>
                {
                    try
                    {
                        switch (item.MenuType)
                        {
                            case MenuType.Profile:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await _navigation.PushAsync(new UserProfilePage());
                                break;
                            case MenuType.Home:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await _navigation.PushAsync(new MainAdminMasterDetailPage());
                                break;
                            case MenuType.WhyWe:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await _navigation.PushAsync(new WhyWePage());
                                break;
                            case MenuType.ContactUs:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await _navigation.PushAsync(new ContactUsPage());
                                break;
                            case MenuType.Share:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await TapOnShare();
                                break;
                            case MenuType.Notification:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await TapOnNotification();
                                break;
                            case MenuType.Logout:
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.MasterPage.IsPresented = false;
                                await TapOnLogout();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception exception)
                    {
                    }
                    finally
                    {
                    }
                });
            }
        }
        #endregion

        #region Methods             
        public async void FillMenuItems()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    //add code to get pending services count
                    string notificationCount = string.Empty;
                    BannerImages _bannerImg = new BannerImages();
                    //get db data to load in banner

                    HttpClientHelper apicall = new HttpClientHelper(string.Format(ApiUrls.Url_GetPendingServiceNotification), Settings.AccessTokenSettings);
                    var response = await apicall.GetResponse<string>();
                    if (!string.IsNullOrEmpty(response))
                    {
                        notificationCount = response;
                    }

                    menuItems = new List<DrawerMenuItem>
                {
                    new DrawerMenuItem { Title = "Profile", MenuType = MenuType.Profile, Icon = "usericon.png",TargetType = null, IsCount= "0", IsFrameVisible=false },
                     new DrawerMenuItem { Title = "Home", MenuType = MenuType.Home, Icon = "home.png",TargetType = null , IsCount= "0", IsFrameVisible=false},
                     new DrawerMenuItem { Title = "Why We?", MenuType = MenuType.WhyWe, Icon = "Whywe.png",TargetType = null, IsCount= "0", IsFrameVisible=false },
                     new DrawerMenuItem { Title = "Contact Us", MenuType = MenuType.ContactUs, Icon = "contacticon.png",TargetType = null, IsCount= "0", IsFrameVisible=false },
                     new DrawerMenuItem { Title = "Share", MenuType = MenuType.Share, Icon = "share.png",TargetType = null , IsCount= "0", IsFrameVisible=false},
                     new DrawerMenuItem { Title = "Notification", MenuType = MenuType.Notification, Icon = "notification.png",TargetType = null , IsCount= notificationCount, IsFrameVisible=true},
                     new DrawerMenuItem { Title = "LogOut", MenuType = MenuType.Logout, Icon = "logout.png",TargetType = null , IsCount= "0", IsFrameVisible=false}
                };
                    DrawerMenuList = menuItems;
                    UserProfileImage = "logo.png";
                    UserName = Settings.DisplayNameSettings;
                    Email = string.IsNullOrEmpty(Cache.GlobalEmail) ? Settings.DisplayEmailSettings : Cache.GlobalEmail;
                    UserDialogs.Instance.HideLoading();
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
        private async void TapOnBackImage(object parma)
        {
            try
            {
                Cache.MasterPage.IsPresented = false;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        private async Task TapOnLogout()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                {
                    var result = await Application.Current.MainPage.DisplayAlert("LogOut Alert!", "Do you really want to logout?", "Yes", "No");
                    if (result)
                    {
                        UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                        //add code to logout user
                        Settings.AccessTokenSettings = string.Empty;
                        Settings.RenewalTokenSettings = string.Empty;
                        Settings.DisplayNameSettings = string.Empty;
                        Settings.DisplayEmailSettings = string.Empty;
                        await _navigation.PushAsync(new LoginPage());
                        UserDialogs.Instance.HideLoading();
                    }
                });
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
        private async Task TapOnShare()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    string AppUrl = "https://play.google.com/store/apps/details?id=com.deepmindsinfotech.urbanservices";
                    var shareOption = DependencyService.Get<IShare>();
                    shareOption.ShareImage("Share Url", AppUrl);
                    UserDialogs.Instance.HideLoading();
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

        private async Task TapOnNotification()
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Cache.goToBackButtonText = "MainAdminHomePage";
                    UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                    await _navigation.PushAsync(new NotificationPage("Admin"));
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
        #endregion
    }
}
