using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.RequestModels
{
    public class ProductComplaintReqModel
    {        
        public string complaintSummary { get; set; }
        public string catagoryName { get; set; }
        public string productName { get; set; }
        public DateTime complaintDate { get; set; }
    }
}
