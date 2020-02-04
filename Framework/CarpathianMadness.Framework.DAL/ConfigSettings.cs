
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CarpathianMadness.Framework.DAL
{
    /// <summary>
    /// Represents all the application configuration settings for the service.
    /// </summary>
    public static class ConfigSettings
    {
        #region Members

        #endregion Members

        #region Properties

        #endregion Properties

        #region Constructors

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Adds database contexts provided in the running application's configuration file.
        /// </summary>
        public static void SetupAppSettingsFromConfig()
        {
            SetupConfigSettings();
        }

        #endregion Public Methods

        #region Private Methods

        private static void SetupConfigSettings()
        {

        }

        private static string GetSetting(string settingName)
        {
            if (!string.IsNullOrWhiteSpace(settingName))
                try
                {
                    if (System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains(settingName))
                        return System.Configuration.ConfigurationManager.AppSettings[settingName];
                }
                catch
                {
                }
            return string.Empty;
        }

        private static int GetSettingAsInt(string settingName)
        {
            int result = 0;
            if (int.TryParse(GetSetting(settingName), out result))
                return result;
            return 0;
        }

        private static bool GetSettingAsBool(string settingName)
        {
            switch (GetSetting(settingName).ToLower())
            {
                case "1":
                case "-1":
                case "true":
                case "yes":
                case "on":
                    return true;
                default:
                    return false;
            }
        }

        #endregion Private Methods
    }
}
