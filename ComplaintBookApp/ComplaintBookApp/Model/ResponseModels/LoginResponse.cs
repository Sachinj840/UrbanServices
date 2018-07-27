using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
    public class LoginResponse
    {
        public string userName { get; set; }
        public string access_token { get; set; }
        public string Message { get; set; }
    }
}
