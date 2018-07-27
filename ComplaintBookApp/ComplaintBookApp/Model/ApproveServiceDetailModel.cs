using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model
{
    public class ApproveServiceDetailModel
    {
        public string Complaint { get; set; }
        public string ComplaintId { get; set; }
        public string Catagory { get; set; }
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Status { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string EmailId { get; set; }
        public DateTime ComplaintDate { get; set; }
    }
}
