using Acr.UserDialogs;
using ComplaintBookApp.Common;
using ComplaintBookApp.Common.Enumerators;
using ComplaintBookApp.Constants;
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

namespace ComplaintBookApp.ViewModel
{
    public class BaseViewModel : NotifyPropertiesChanged
    {
        #region Data Members
        private INavigation _navigation;
        private static ICommand backCommand;
        private static ICommand skipCommand;
        #endregion

        #region Constructor
        public BaseViewModel(INavigation navigation)
        {
            _navigation = navigation;
            backCommand = BackCommandStore;
        }

        #endregion

        #region Properties  

        private Color approveBackgroundColor;
        public Color ApproveBackgroundColor
        {
            get { return approveBackgroundColor; }
            set { approveBackgroundColor = value; OnPropertyChanged(); }
        }

        private Color inprogressBackgroundColor;
        public Color InProgressBackgroundColor
        {
            get { return inprogressBackgroundColor; }
            set { inprogressBackgroundColor = value; OnPropertyChanged(); }
        }

        private Color _matchColor;
        public Color MatchColor
        {
            get { return _matchColor; }
            set { _matchColor = value; OnPropertyChanged(); }
        }

        private Color _homeColor;
        public Color HomeColor
        {
            get { return _homeColor; }
            set { _homeColor = value; OnPropertyChanged(); }
        }

        private Color _searchColor;
        public Color SearchColor
        {
            get { return _searchColor; }
            set { _searchColor = value; OnPropertyChanged(); }
        }

        private Color _chatColor;
        public Color ChatColor
        {
            get { return _chatColor; }
            set { _chatColor = value; OnPropertyChanged(); }
        }

        private ImageSource _homeIcon;
        public ImageSource HomeIcon
        {
            get { return _homeIcon; }
            set { _homeIcon = value; OnPropertyChanged(); }
        }

        private ImageSource _matchIcon;
        public ImageSource MatchIcon
        {
            get { return _matchIcon; }
            set { _matchIcon = value; OnPropertyChanged(); }
        }

        private ImageSource _searchIcon;
        public ImageSource SearchIcon
        {
            get { return _searchIcon; }
            set { _searchIcon = value; OnPropertyChanged(); }
        }

        private ImageSource _chatIcon;
        public ImageSource ChatIcon
        {
            get { return _chatIcon; }
            set { _chatIcon = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand
        {
            get { return backCommand; }
            set { backCommand = value; OnPropertyChanged(); }
        }

        public ICommand SkipCommand
        {
            get { return skipCommand; }
            set { skipCommand = value; OnPropertyChanged(); }
        }

        private static Color matchsBackgroundColor;
        public Color MatchsBackgroundColor
        {
            get { return matchsBackgroundColor; }
            set { matchsBackgroundColor = value; OnPropertyChanged(); }
        }

        private static Color newMatchBackgroundColor;
        public Color NewMatchBackgroundColor
        {
            get { return newMatchBackgroundColor; }
            set { newMatchBackgroundColor = value; OnPropertyChanged(); }
        }

        private static Color premiumBackgroundColor;
        public Color PremiumBackgroundColor
        {
            get { return premiumBackgroundColor; }
            set { premiumBackgroundColor = value; OnPropertyChanged(); }
        }

        private static Color recentBackgroundColor;
        public Color RecentBackgroundColor
        {
            get { return recentBackgroundColor; }
            set { recentBackgroundColor = value; OnPropertyChanged(); }
        }

        private static Color viewedProfileBackgroundColor;
        public Color ViewedProfileBackgroundColor
        {
            get { return viewedProfileBackgroundColor; }
            set { viewedProfileBackgroundColor = value; OnPropertyChanged(); }
        }

        //

        private bool isRunningTasks;
        /// <summary>
        /// /Get or set the IsRunningTasks
        /// </summary>
        public bool IsRunningTasks
        {
            get { return isRunningTasks; }
            set { isRunningTasks = value; OnPropertyChanged(); }
        }

        private string pageHeaderText;

        public string PageHeaderText
        {
            get { return pageHeaderText.ToUpper(); }
            set { pageHeaderText = value; OnPropertyChanged(); }
        }


        private string pagesubHeaderText;

        public string PageSubHeaderText
        {
            get { return pagesubHeaderText; }
            set { pagesubHeaderText = value; OnPropertyChanged(); }
        }

        private string headerTitleText;

        public string HeaderTitleText
        {
            get { return headerTitleText.ToUpper(); }
            set { headerTitleText = value; OnPropertyChanged(); }
        }

        private bool isBackButtonVisible;
        public bool IsBackButtonVisible
        {
            get { return isBackButtonVisible; }
            set { isBackButtonVisible = value; OnPropertyChanged(); }
        }

        private bool isMenuVisible;
        public bool IsMenuVisible
        {
            get { return isMenuVisible; }
            set { isMenuVisible = value; OnPropertyChanged(); }
        }

        private bool isSkipVisible;
        public bool IsSkipVisible
        {
            get { return isSkipVisible; }
            set { isSkipVisible = value; OnPropertyChanged(); }
        }

        public Action DisplayMessage;
        public Action OnPageNavigation;
        public Action<object> OnConditionNavigation;

        #endregion

        #region Delegate Commonds
        public ICommand FooterCommand
        {
            get { return new Command<string>(OnNavigation); }
        }

        #endregion

        #region Methods

        protected async void OnNavigation(string param)
        {
            try
            {
                if (string.IsNullOrEmpty(param))
                {
                    return;
                }

                App.IsNavigating = true;
                var pageType = (ApplicationActivity)Enum.Parse(typeof(ApplicationActivity), param);
                switch (pageType)
                {
                    case ApplicationActivity.ProductListPage:
                        await PageNavigation(ApplicationActivity.ProductListPage, pageType);
                        break;                    
                    case ApplicationActivity.CheckInprogressServiceStatusListPage:
                        await PageNavigation(ApplicationActivity.CheckInprogressServiceStatusListPage, pageType);
                        break;
                    case ApplicationActivity.CheckApprovedServiceStatusListPage:
                        await PageNavigation(ApplicationActivity.CheckInprogressServiceStatusListPage, pageType);
                        break;
                    default:
                        break;
                }
                
            }
            catch (Exception ex)
            {
            }
        }

        protected async Task PageNavigation(ApplicationActivity page, ApplicationActivity pageType)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.ShowLoading("Loading..."));
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    switch (page)
                    {
                        case ApplicationActivity.ProductListPage:
                            await _navigation.PushAsync(new ProductListPage(pageType));
                           // UserDialogs.Instance.HideLoading();
                            break;
                        case ApplicationActivity.CheckApprovedServiceStatusListPage:
                            ApproveBackgroundColor = Color.FromHex("#981C1C");
                            InProgressBackgroundColor = Color.FromHex("#000000");
                            await _navigation.PushAsync(new CheckApprovedServiceStatusListPage(pageType));
                            // UserDialogs.Instance.HideLoading();
                            break;
                        case ApplicationActivity.CheckInprogressServiceStatusListPage:
                            ApproveBackgroundColor = Color.FromHex("#981C1C");
                            InProgressBackgroundColor = Color.FromHex("#000000");
                            await _navigation.PushAsync(new CheckInprogressServiceStatusListPage(pageType));
                            // UserDialogs.Instance.HideLoading();
                            break;
                    }
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
                    Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.AlertAsync("Network", AppConstant.NETWORK_FAILURE, "OK"));
                    return;
                }                
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() => UserDialogs.Instance.HideLoading());
            }
        }

        public ICommand BackCommandStore
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        switch (Cache.goToBackButtonText)
                        {
                            case "HomePage":
                                Cache.goToBackButtonText = string.Empty;
                                await _navigation.PushAsync(new HomePage());
                                break;
                            case "LoginPage":
                                Cache.goToBackButtonText = "HomePage";
                                await _navigation.PushAsync(new LoginPage());
                                break;
                            case "MainHomePage":
                                Cache.globalProduct = string.Empty;
                                Cache.globalCatagory = string.Empty;
                                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                                await _navigation.PushAsync(new MainMasterDetailPage());
                                break;
                            case "MainAdminHomePage":
                                Cache.globalProduct = string.Empty;
                                Cache.globalCatagory = string.Empty;
                                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                                await _navigation.PushAsync(new MainAdminMasterDetailPage());
                                break;
                            case "ApproveComplaintPage":
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.GlobalUserId = string.Empty;
                                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);
                                await _navigation.PushAsync(new ApproveServiceComplaintListPage());
                                break;
                            case "StatusComplaintPage":
                                Cache.goToBackButtonText = "MainAdminHomePage";
                                Cache.GlobalUserId = string.Empty;
                                UserDialogs.Instance.ShowLoading("Loading...", MaskType.Black);                                
                                await _navigation.PushAsync(new CheckApprovedServiceStatusListPage(ApplicationActivity.CheckApprovedServiceStatusListPage));
                                break;                                                      
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }
        #endregion
    }
}
