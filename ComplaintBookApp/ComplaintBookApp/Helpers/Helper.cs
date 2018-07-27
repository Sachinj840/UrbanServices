using ComplaintBookApp.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ComplaintBookApp.Helpers
{
    public class Helper
    {

        public static bool IsEmailValid(string emailaddress)
        {
            try
            {
                Regex regex = new Regex(AppConstant.emailRegex);
                Match match = regex.Match(emailaddress);
                if (match.Success)
                    return true;
                else
                    return false;
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static string GetCurrentDate(string format)
        {
            string currentDate = DateTime.Now.Date.ToString(format);
            return currentDate;
        }
    }
}
