using ComplaintBookApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
    public class BannerResponse
    {
        public List<US_BANNER_IMAGE> banner { get; set; }
        public List<US_BANNERSUB_IMAGE> subOneBanner { get; set; }
        public List<US_BANNERSUB_IMAGE> subTwoBanner { get; set; }
    }
}
