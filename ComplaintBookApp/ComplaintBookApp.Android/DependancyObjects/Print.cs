using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.PrintServices;
using ComplaintBookApp.Common.Interfaces;
using Android.Webkit;
using Xamarin.Forms.Platform.Android;
using Android.Print;
using Xamarin.Forms;
using ComplaintBookApp.Droid.DependancyObjects;

[assembly: Xamarin.Forms.Dependency(typeof(Print))]
namespace ComplaintBookApp.Droid.DependancyObjects
{
    public class Print : IPrint
    {
        public void PrintInfo(Xamarin.Forms.WebView Data)
        {
            var droidViewToPrint = Platform.CreateRenderer(Data).ViewGroup.GetChildAt(0) as Android.Webkit.WebView;

            if (droidViewToPrint != null)
            {
                // Only valid for API 19+
                var version = Android.OS.Build.VERSION.SdkInt;

                if (version >= Android.OS.BuildVersionCodes.Kitkat)
                {
                    var printMgr = (PrintManager)Forms.Context.GetSystemService(Context.PrintService);

                    printMgr.Print("Forms-EZ-Print", droidViewToPrint.CreatePrintDocumentAdapter(), null);
                }
            }
        }
    }
}