using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.Model
{
    public class BannerImages
    {
        public int ImageId { get; set; }
        public string Image { get; set; }
        public int srNo { get; set; }
        public string Imagelabel { get; set; }
        public string description { get; set; }
    }

    public class SubBannerOneImages
    {
        public int ImageId { get; set; }
        public string SubOneImage { get; set; }
        public int srNo { get; set; }
        public string SubOneImagelabel { get; set; }
        public string description { get; set; }
        public string Catagory { get; set; }
    }

    public class SubBannerTwoImages
    {
        public int ImageId { get; set; }
        public string SubTwoImage { get; set; }
        public int srNo { get; set; }
        public string SubTwoImagelabel { get; set; }
        public string description { get; set; }
        public string Catagory { get; set; }
    }
}
