using ComplaintBookApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.RequestModels
{
    public class RegisterReqModel
    {
        public int user_profile_id { get; set; }
        public int UserID { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string ProfilePicture { get; set; }
        public DateTime? date_modified { get; set; }
        public string password { get; set; }
        public List<US_CITY> lstCity { get; set; }
        public List<US_STATE> lstState { get; set; }
    }
}
