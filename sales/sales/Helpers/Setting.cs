

namespace sales.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Setting
    {


        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string tokenType = "TokenType";
        private const string accessToken = "AccessToken";
        private const string isRememberd = "IsRemembered";
        private const string userRequest = "UserRequest";
        private static readonly string stringDefault = string.Empty;
        private static readonly bool boolDefault = false;




        #endregion


        public static string UserRequest
        {
            get
            {
                return AppSettings.GetValueOrDefault(userRequest, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userRequest, value);
            }
        }


        public static string TokenType
        {
            get
            {
                return AppSettings.GetValueOrDefault(tokenType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(tokenType, value);
            }
        }


        public static string AccessToken
        {
            get
            {
                return AppSettings.GetValueOrDefault(accessToken, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(accessToken, value);
            }
        }

        public static bool IsRemembered
        {
            get
            {
                return AppSettings.GetValueOrDefault(isRememberd, boolDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(isRememberd, value);
            }
        }
    }
}
