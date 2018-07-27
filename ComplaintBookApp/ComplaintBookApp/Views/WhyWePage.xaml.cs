using ComplaintBookApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComplaintBookApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WhyWePage : ContentPage
    {
        public WhyWeViewModel whyWeVM;
        public WhyWePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            whyWeVM = new WhyWeViewModel(Navigation);
            BindingContext = whyWeVM;
        }
    }
}