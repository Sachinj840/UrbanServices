using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model
{
    public class ApproveServiceModel: BaseModel
    {                                                
        private DateTime date_complaint;        
        private string complaintDate;

        public int complaintId { get; set; }
        public string Catagory { get; set; }
        public string ProductName { get; set; }
        public string ComplaintSummary { get; set; }
        public string status { get; set; }
        public string ComplaintDate
        {
            get { return complaintDate; }
            set
            {
                complaintDate = value; OnPropertyChanged();                
            }
        }

        public DateTime Date_Complaint
        {
            get { return date_complaint; }
            set
            {
                date_complaint = value;
                ComplaintDate = value.ToString("MM/dd/yy");
            }
        }        
    }
}
