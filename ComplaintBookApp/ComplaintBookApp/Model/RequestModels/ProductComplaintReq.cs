using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.RequestModels
{
    public class ProductComplaintReq
    {        
        public string complaintSummary { get; set; }
        public string catagory { get; set; }
        public string productId { get; set; }
        public DateTime complaintDate { get; set; }
    }
}
