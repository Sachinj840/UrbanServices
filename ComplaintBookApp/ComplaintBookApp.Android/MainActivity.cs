using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using Plugin.Connectivity;
using XLabs.Ioc;

namespace ComplaintBookApp.Droid
{
    //Theme = "@style/MainTheme"
    [Activity(Label = "UrbanServices", Icon = "@drawable/icon", Theme = "@style/Theme.Splash", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.SetTheme(Resource.Style.MainTheme);
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            var container = new SimpleContainer();
            
            container.Register(CrossConnectivity.Current);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);
            LoadApplication(new App());
        }
    }
}

