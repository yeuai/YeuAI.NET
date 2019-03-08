using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;

namespace YeuAI.Common.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class AppLogger
    {
        /// <summary>
        /// Default logger name
        /// </summary>
        public const string DefaultLoggerName = "Main";

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
                var logLayout = AppConfig.Get("LogLayout", "${date:format=dd/MM/yyy HH\\:mm\\:ss}|${threadid}|${level}|${logger}|${stacktrace}|${message}");
                var logFileName = AppConfig.Get("LogFileName", "${basedir}/Logs/${date:format=yyyy}/${date:format=MM}/${date:format=dd}/${logger}.log");
                var logArchiveAboveSize = AppConfig.Get("LogArchiveAboveSize", "5242880");
                var logSize = int.Parse(logArchiveAboveSize);

                // log Error
                var logErrorLayout = AppConfig.Get("LogErrorLayout", "${date:format=dd/MM/yyy HH\\:mm\\:ss}|${threadid}|${level}|${logger}|${stacktrace}|${message}");
                var logErrorFileName = AppConfig.Get("LogErrorFileName", "${basedir}/Logs/${date:format=yyyy}/${date:format=MM}/${date:format=dd}/error.log");
                var logErrorArchiveAboveSize = AppConfig.Get("LogErrorArchiveAboveSize", "5242880");
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

        /// <summary>
        /// Support quick reference from object with name of type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Logger GetLogger(this object obj)
        {
            if (obj == null)
            {
                return GetLogger();
            }
            else
            {
                return GetLogger(obj.GetType().Name);
            }
        }

        /// <summary>
        /// Extension logging with default logger
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(this Exception ex)
        {
            GetLogger().Error(ex, "Có lỗi xảy ra!");
        }
    }
}
