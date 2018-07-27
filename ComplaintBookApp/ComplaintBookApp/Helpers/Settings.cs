// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace ComplaintBookApp.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

        private const string DisplayName = "userName";
        private static readonly string DefaultDisplayName = string.Empty;

        private const bool IsFirstTime = false;
        private static readonly bool DefaultIsFirstTime = false;

        private const string DisplayEmail = "email";
        private static readonly string DefaultDisplayEmail = string.Empty;

        private const string AccessToken = "access_token";
        private static readonly string DefaultAccessToken = string.Empty;

        private const string RenewalToken = "renewal_token";
        private static readonly string DefaultRenewalToken = string.Empty;

        #endregion

        #region Getter and Setter

        public static string DisplayEmailSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(DisplayEmail, DefaultDisplayEmail);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DisplayEmail, value);
            }
        }
        public static string DisplayNameSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(DisplayName, DefaultDisplayName);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DisplayName, value);
            }
        }
        public static bool IsFirstTimeLoginSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsFirstTime.ToString(), DefaultIsFirstTime, null);
            }
            set
            {
                AppSettings.AddOrUpdateValue(DisplayName, value);
            }
        }
        public static string RenewalTokenSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(RenewalToken, DefaultRenewalToken);
            }
            set
            {
                try
                {
                    AppSettings.AddOrUpdateValue(RenewalToken, value);
                }
                catch (Exception e)
                {
                    AppSettings.Remove(RenewalToken);
                    AppSettings.AddOrUpdateValue(RenewalToken, value);
                }
            }
        }
        public static string AccessTokenSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(AccessToken, DefaultAccessToken);
            }
            set
            {
                AppSettings.AddOrUpdateValue(AccessToken, value);
            }
        }
        public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

        #endregion

    }
}