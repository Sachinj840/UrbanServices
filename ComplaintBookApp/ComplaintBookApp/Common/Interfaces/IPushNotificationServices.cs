using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Common.Interfaces
{
    public interface IPushNotificationServices
    {
        void RegisterPushNotificationServices();

        void UnRegisterPushNotificationServices();
    }
}
