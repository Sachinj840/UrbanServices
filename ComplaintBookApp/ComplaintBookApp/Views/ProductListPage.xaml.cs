using ComplaintBookApp.Common.Enumerators;
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
	public partial class ProductListPage : ContentPage
	{
        public ProductListPageViewModel productListPageVM;
        public ProductListPage (ApplicationActivity pageType)
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            productListPageVM = new ProductListPageViewModel(Navigation, pageType);
            BindingContext = productListPageVM;
        }
	}
}