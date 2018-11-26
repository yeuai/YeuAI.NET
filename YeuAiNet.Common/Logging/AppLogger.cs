using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;

namespace YeuAiNet.Common.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppLogger
    {
        /// <summary>
        /// Default logger name
        /// </summary>
        public const string DefaultLoggerName = "app";

        /// <summary>
        /// Static constructor
        /// </summary>
        static AppLogger()
        {
            Config();
        }

        /// <summary>
        /// Cấu hình log cho toàn bộ ứng dụng
        /// </summary>
        /// <returns></returns>
        private static void Config()
        {
            try
            {
                if (File.Exists("NLog.config"))
                {
                    GetLogger().Debug("Logging use NLog.config file");
                    return;
                }
                else
                {
                    Console.WriteLine("Config NLog ...");
                }
                // log Info
                var logLayout = AppConfiguration.Get("LogLayout", "${date:format=dd/MM/yyy HH\\:mm\\:ss}|${threadid}|${level}|${logger}|${stacktrace}|${message}");
                var logFileName = AppConfiguration.Get("LogFileName", "${basedir}/Logs/${date:format=yyyy}/${date:format=MM}/${date:format=dd}/${logger}.log");
                var logArchiveAboveSize = AppConfiguration.Get("LogArchiveAboveSize", "5242880");
                var logSize = int.Parse(logArchiveAboveSize);

                // log Error
                var logErrorLayout = AppConfiguration.Get("LogErrorLayout", "${date:format=dd/MM/yyy HH\\:mm\\:ss}|${threadid}|${level}|${logger}|${stacktrace}|${message}");
                var logErrorFileName = AppConfiguration.Get("LogErrorFileName", "${basedir}/Logs/${date:format=yyyy}/${date:format=MM}/${date:format=dd}/error.log");
                var logErrorArchiveAboveSize = AppConfiguration.Get("LogErrorArchiveAboveSize", "5242880");
                var logErrorSize = int.Parse(logErrorArchiveAboveSize);


                var config = new LoggingConfiguration();
                var logDetailFileTarget = new FileTarget
                {
                    FileName = logFileName,
                    Layout = logLayout,
                    ArchiveAboveSize = logSize
                };
                var logErrorFileTarget = new FileTarget
                {
                    FileName = logErrorFileName,
                    Layout = logErrorLayout,
                    ArchiveAboveSize = logErrorSize
                };

                config.AddTarget("file", logDetailFileTarget);
                config.AddTarget("error", logErrorFileTarget);
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, logDetailFileTarget));
                config.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, logErrorFileTarget));
                LogManager.Configuration = config;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Init log msg: " + ex.Message);
                Console.WriteLine("ERROR Init log trace: " + ex.StackTrace);
                throw ex;
            }
        }

        /// <summary>
        /// Get logger theo tên và loại
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Logger GetLogger(string name, Type type)
        {
            return LogManager.GetLogger(name, type);
        }

        /// <summary>
        /// Get logger theo tên
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Logger GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }

        /// <summary>
        /// Tạo logger mặc định theo với tên: EHR
        /// </summary>
        /// <returns></returns>
        public static Logger GetLogger()
        {
            return GetLogger(DefaultLoggerName);
        }
    }
}
