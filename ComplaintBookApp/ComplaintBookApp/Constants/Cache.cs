using ComplaintBookApp.Model.ResponseModels;
using ComplaintBookApp.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComplaintBookApp.Constants
{
    public class Cache
    {
        public static string access_token { get; set; }
        public static string goToBackButtonText { get; set; }
        public static string globalOTP { get; set; }
        public static string globalCatagory { get; set; }
        public static string globalProduct { get; set; }
        public static MasterDetailPage MasterPage { get; set; }
        public static List<US_CITY> GlobalCity{get;set;}
        public static string GlobalEmail { get; set; }
        public static string userEmail { get; set; }
        public static string GlobalUserId { get; set; }
        public static string GlobalComplaintId { get; set; }
        public static string DisplayName { get; set; }
        
    }
}
