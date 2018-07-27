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
	public partial class ServiceCommentListPage : ContentPage
    {
        public ServiceCommentListPageViewModel serviceComplaintListPageVM;
        public ServiceCommentListPage ()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            serviceComplaintListPageVM = new ServiceCommentListPageViewModel(Navigation);
            BindingContext = serviceComplaintListPageVM;
        }
	}
}