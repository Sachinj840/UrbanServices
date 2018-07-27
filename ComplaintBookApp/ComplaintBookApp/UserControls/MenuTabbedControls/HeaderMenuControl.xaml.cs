using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintBookApp.UserControls.MenuTabbedControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeaderMenuControl : ContentView
    {
        public HeaderMenuControl()
        {
            InitializeComponent();
        }
    }
}