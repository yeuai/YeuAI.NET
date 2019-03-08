using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

namespace YeuAI.Common
{
    /// <summary>
    /// Application Configuration Manager
    /// </summary>
    public static class AppConfig
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
        public static string GetConnectionString(string configName)
        {
            return ConfigurationManager.ConnectionStrings[configName].ConnectionString;
        }

        /// <summary>
        /// Get application startup path
        /// </summary>
        public static string ApplicationStartupPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        /// <summary>
        /// Get path from appliction startup path
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string GetPath(params string[] paths)
        {
            if (paths == null)
            {
                return ApplicationStartupPath;
            }
            else
            {
                var vPaths = (new string[] { ApplicationStartupPath }).Concat(paths).ToArray();
                return Path.Combine(vPaths);
            }
        }
    }
}
