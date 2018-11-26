using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace YeuAiNet.Common
{
    public static class AppConfiguration
    {
        /// <summary>
        /// Get AppSettings section data
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string Get(string configName)
        {
            return ConfigurationManager.AppSettings[configName];
        }

        /// <summary>
        /// Get AppSettings section data or default value
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string Get(string configName, string defaultValue)
        {
            var configValue = ConfigurationManager.AppSettings[configName];
            return string.IsNullOrEmpty(configValue) ? defaultValue : configValue;
        }

        /// <summary>
        /// Get ConnectionString section data
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnection(string configName)
        {
            return ConfigurationManager.ConnectionStrings[configName].ConnectionString;
        }
    }
}
