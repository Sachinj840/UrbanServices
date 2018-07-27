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
using ComplaintBookApp.Droid.DependancyObjects;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Java.IO;
using ComplaintBookApp.Common.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(Share))]
namespace ComplaintBookApp.Droid.DependancyObjects
{
    public class Share : IShare
    {
        public void ShareImage(string message, string imagePath)
        {
            var intent = new Intent(Intent.ActionSend);
            intent.PutExtra(Intent.ExtraText, imagePath);
            intent.PutExtra(Intent.ExtraSubject, message);
            intent.SetType("text/plain");          
            Forms.Context.StartActivity(Intent.CreateChooser(intent, "Share Url"));
        }
    }
}