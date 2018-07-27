using ComplaintBookApp.Common.Interfaces;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.Common.Services
{
    public class CameraService : ICameraService
    {
        private readonly IMedia media;

        public CameraService(IMedia media)
        {
            this.media = media;
            media.Initialize();
        }

        public async Task<bool> CheckPermissions()
        {
            //var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (storageStatus != PermissionStatus.Granted)//cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                    storageStatus = results[Permission.Storage];
                    //cameraStatus = results[Permission.Camera];
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                        storageStatus = results[Permission.Storage];
                        //cameraStatus = results[Permission.Camera];	
                    });
                }
            }

            return storageStatus != PermissionStatus.Granted;//cameraStatus != PermissionStatus.Granted && storageStatus != PermissionStatus.Granted;
        }

        public async Task<MediaFile> PickFileAsync()
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Permissions Denied. Please modify app permisions in settings.", "Unable to pick a file.", "OK");
                return null;
            }

            if (!media.IsPickPhotoSupported)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("No Gallery", "Picking a photo is not supported.", "OK");
                return null;
            }

            var file = await media.PickPhotoAsync();
            return file == null ? null : file;
        }

        public async Task<MediaFile> TakePhotoAsync()
        {
            var denied = await CheckPermissions();
            if (denied)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Permissions Denied. Please modify app permisions in settings.", "Unable to take photos.", "OK");
                return null;
            }

            if (!media.IsCameraAvailable || !media.IsTakePhotoSupported)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("No Camera", "No camera available!", "OK");
                return null;
            }

            MediaFile file = null;
            try
            {
                file = await media.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = true,
                    //Name = $"{folderName} {DateTime.Now}",
                    //Directory = AppSetting.APP_COMPANY,
                    SaveMetaData = true
                });
            }
            catch (Exception ex)
            {
            }

            return file == null ? null : file;
        }
    }
}
