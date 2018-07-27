using ComplaintBookApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
   public class ProductListResponseModel
    {
        public List<US_ELECTRICAL_SERVICE> electrical { get; set; }
        public List<US_ELECTRONICS_SERVICE> electronic { get; set; }
        public List<US_IT_SERVICE> it { get; set; }
        public List<US_DAILY_NEEDS> dailyNeeds { get; set; }
    }
}
