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
using ComplaintBookApp.Droid.Renderer;
using ComplaintBookApp.UserControls.CustomControls;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]
namespace ComplaintBookApp.Droid.Renderer
{
    public class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            //this.Control.Background = Forms.Context.GetDrawable(Resource.Drawable.CustomPickerBackground);
            Control?.SetPadding(20, 20, 20, 20);            
            if (e.OldElement != null || e.NewElement != null)
            {
                var customPicker = e.NewElement as BindablePicker;                
                Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));
            }
        }
    }
}