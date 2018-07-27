using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Tables
{
    public class US_TUSER_PROFILE
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
        public bool IsAdmin { get; set; }        

    }
}
