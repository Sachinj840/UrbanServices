using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.ViewModel
{
    public class WhyWeViewModel : BaseViewModel
    {
        #region Data Members
        private INavigation _navigation;
        #endregion

        #region Constructor
        public WhyWeViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            PageHeaderText = "Why We?";
            IsBackButtonVisible = true;
            IsMenuVisible = false;            
        }
        #endregion

        #region Property        

        #endregion

        #region Delegate Commonds       

        #endregion

        #region Methods      
        #endregion
    }
}
