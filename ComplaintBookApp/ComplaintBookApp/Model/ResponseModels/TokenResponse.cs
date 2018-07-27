using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model.ResponseModels
{
    public class TokenResponse
    {
        public string displayName { get; set; }
        public string accessToken { get; set; }
        public string renewalToken { get; set; }
    }
}
