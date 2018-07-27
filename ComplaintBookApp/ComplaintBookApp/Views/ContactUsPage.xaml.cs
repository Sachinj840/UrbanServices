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
    public partial class ContactUsPage : ContentPage
    {
        public ContactUsViewModel contactVM; 
        public ContactUsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            contactVM = new ContactUsViewModel(Navigation);
            BindingContext = contactVM;
        }
    }
}