using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintBookApp.Constants
{
    public class ApiUrls
    {
        //User Registration
        public const string Url_User_Register = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/Register";
        public const string Url_User_SendOTP = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/SendOtpEmail";
        public const string Url_GetDropDownListData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/GetDropDownListData";

        //Get Productdata
        public const string Url_GetProductListData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetProductListData";
        public const string Url_RegisterComplaint = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/RegisterComplaint";

        //get user profile Data
        public const string Url_UserProfileData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/GetUserData?UserId={0}";
        public const string Url_AllUserProfileData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/GetAllUserData";
        public const string Url_UpdateUserProfileData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/UpdateUserData";

        //register complaint
        public const string Url_Register_Complaint = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/RegisterComplaint";
        public const string Url_Approve_Complaint = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/ApproveComplaint";

        //reset/forgot pass
        public const string Url_User_ResetPassword = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/ResetPassword?emailId={0}";

        //email to admin
        public const string Url_Send_EmailAdmin = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/SendEmail";

        //email to user
        public const string Url_Send_EmailUser = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanUser/SendEmailToUser?emailId={0}&serviceName={1}&catagory={2}";

        //get banner image data
        public const string Url_GetbannerImages = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetBannerImages?IsMain={0}";
        public const string Url_GetSubbannerImages = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetSubBannerImages";

        //save banner images
        public const string Url_SaveBannerImages = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/SaveBannerImages";
        public const string Url_SaveSubBannerImages = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/SaveSubBannerImages";

        //delete banner images
        public const string Url_DeleteBannerImages = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/DeleteBannerImages?imageId={0}&IsMain={1}";


        //get pending service data
        public const string Url_GetPendingServiceList = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetPendingServiceList";
        public const string Url_GetPendingServiceData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetPendingServiceData?complaintId={0}&status={1}";

        //get Approved/inprogress service data
        public const string Url_GetApprovedInProgressServiceList = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetApprovedInProgressServiceList";

        //get notification count data
        public const string Url_GetPendingServiceNotification = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetPendingServiceNotification";
        public const string Url_GetApprovedServiceNotification = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetApprovedServiceNotification";
        public const string Url_GetAdminNotificationData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetAdminNotificationData";
        public const string Url_GetUserNotificationData = AppSetting.Base_Url + "UrbanServicesPlatform/API/" + AppSetting.API_VERSION + "/UrbanProduct/GetUserNotificationData";        

        //JwtAuth
        public const string Url_JwtAuth_login = AppSetting.Base_Url + "JwtAuth/API/mobile/login";
        public const string Url_JwtAuth_extendtoken = AppSetting.Base_Url + "JwtAuth/API/mobile/extendtoken";        
    }
}
