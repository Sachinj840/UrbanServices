using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
    public class ResetPasswordResponseModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
}
