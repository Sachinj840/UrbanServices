using ComplaintBookApp.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
    public class ListResponse
    {
        public List<US_CITY> city { get; set; }
        public List<US_STATE> state { get; set; }
    }
}
