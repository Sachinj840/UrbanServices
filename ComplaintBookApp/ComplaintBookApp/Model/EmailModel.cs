using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model
{
    public class EmailModel
    {
        public string customerName { get; set; }
        public string address { get; set; }
        public string emailId { get; set; }
        public string mobileNo { get; set; }
        public DateTime complaintDate { get; set; }
        public string Service { get; set; }
        public string catagory { get; set; }
    }
}
