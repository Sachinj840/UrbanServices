using ComplaintBookApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Model
{
    public class BaseModel : NotifyPropertiesChanged
    {
        public string Title { get; set; }
    }
}
