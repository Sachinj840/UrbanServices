using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintBookApp.Views.PopUpPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfoPrintPopUpPage : PopupPage
    {
        public UserInfoPrintPopUpPage()
        {
            InitializeComponent();
        }

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FrameContainer.HeightRequest = -1;           
        }       

        protected async override void OnDisappearingAnimationBegin()
        {
            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;
            
            FrameContainer.Animate("HideAnimation", d =>
            {
                FrameContainer.HeightRequest = d;
            },
            start: currentHeight,
            end: 170,
            finished: async (d, b) =>
            {
                await Task.Delay(300);
                taskSource.TrySetResult(true);
            });

            await taskSource.Task;
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();

            return false;
        }

        private async void CloseAllPopup()
        {
            await Navigation.PopAllPopupAsync();
        }

        private void OnClose(object sender, EventArgs e)
        {
            PopupNavigation.PopAsync();
        }
        #endregion
    }
}