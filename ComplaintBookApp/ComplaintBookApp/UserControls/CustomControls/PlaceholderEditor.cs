﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.UserControls.CustomControls
{
    public class PlaceholderEditor : Editor
    {
        public static BindableProperty PlaceholderProperty
            = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(PlaceholderEditor));

        public static BindableProperty PlaceholderColorProperty
            = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(PlaceholderEditor), Color.Gray);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }
    }
}
