using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model
{
    public class DrawerMenuItem : BaseModel
    {
        public DrawerMenuItem()
        {
            MenuType = MenuType.Profile;
        }
        public string Icon { get; set; }
        public MenuType MenuType { get; set; }
        public Type TargetType { get; set; }
        public bool IsFrameVisible { get; set; }
        public string IsCount { get; set; }
    }
    public enum MenuType
    {
        Profile,
        Home,        
        WhyWe,        
        ContactUs,
        Share,
        Notification,
        Logout
    }
}
