using ComplaintBookApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
    public class ListResponseModel
    {
        public List<US_CITY> city { get; set; }
        public List<US_COUNTRY> country { get; set; }
        public List<US_STATE> state { get; set; }
    }
}
