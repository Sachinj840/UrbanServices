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
    public partial class BookServiceComplaintPage : ContentPage
    {
        public BookServiceComplaintPageViewModel bookServiceComplaintVM;
        public BookServiceComplaintPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            bookServiceComplaintVM = new BookServiceComplaintPageViewModel(Navigation);
            BindingContext = bookServiceComplaintVM;
        }
        private void OnDateOfBirthFocused(object sender, FocusEventArgs e)
        {
            DateOfBirthEntry.IsVisible = false;
            DOBDatePicker.IsVisible = true;
            Device.BeginInvokeOnMainThread(() =>
            {
                DateOfBirthEntry.Unfocus();
                DOBDatePicker.Focus();
                DateOfBirthEntry.Keyboard = null;
            });
        }
    }
}