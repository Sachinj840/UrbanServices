using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Tables
{
    public class US_PRODUCT_COMPLAINT
    {
        public int complaintId { get; set; }
        public string complaintSummary { get; set; }
        public string catagory { get; set; }
        public string productId { get; set; }
        public DateTime complaintDate { get; set; }
    }
}
