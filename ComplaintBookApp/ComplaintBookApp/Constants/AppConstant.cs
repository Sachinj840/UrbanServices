using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Constants
{
    public class AppConstant
    {
        public const string EMPTY_STRING = "";
        public const string NETWORK_FAILURE = "No Network Connection found! Please try again.";
        public const string LOGIN_FAILURE = "Login Failed! Please try again.";
        public const string REGISTRATION_FAILURE = "Registration Failed! Please try again.";
        public const string Payload = "apppayload";

        /// The email address
        /// </summary>
        public const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
       @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
    }
}
