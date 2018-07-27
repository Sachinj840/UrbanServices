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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using ComplaintBookApp.Droid;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer_Droid))]
namespace ComplaintBookApp.Droid
{
    public class CustomEntryRenderer_Droid : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);

            Control.TextAlignment = TextAlignmentCenter;

        }
    }
}