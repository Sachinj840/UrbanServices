using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using ComplaintBookApp.UserControls.CustomControls;
using ComplaintBookApp.iOS.Renderer;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]
namespace ComplaintBookApp.iOS.Renderer
{
     public class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement != null)
            {
                var customPicker = e.NewElement as BindablePicker;

                string placeholderColor = customPicker.PlaceholderColor;               

                //var placeholderAttributes = new NSAttributedString(customPicker.Title, new UIStringAttributes()
                //{ ForegroundColor = color });

                Control.AttributedPlaceholder = placeholderAttributes;
            }
        }        
    }
}