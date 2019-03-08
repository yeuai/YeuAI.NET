using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using Topshelf;
using Topshelf.Hosts;
using YeuAI.Common;
using YeuAI.Common.Logging;
using YeuAI.WebHost;

namespace YeuAI.WebHost
{
    public class WebServer
    {

        #region Private fields
        private static List<IDisposable> _webapps = new List<IDisposable>();
        #endregion

        /// <summary>
        /// Check application is running in console mode
        /// </summary>
        public static bool IsRunningAsConsole => HostControl is ConsoleRunHost;

        /// <summary>
        /// Voice Cache Hash Prefix length
        /// </summary>
        public static int VoiceCacheHashPrefixLength => int.Parse(AppConfig.Get("Voice.Cache.HashPrefixLength", "3"));

        /// <summary>
        /// Return host control when service is ready
        /// </summary>
        public static HostControl HostControl { get; private set; }

        /// <summary>
        /// Start the web application
        /// </summary>
        /// <param name="hc"></param>
        /// <returns></returns>
        public bool Start(HostControl hc)
        {
            var vHostName = AppConfig.Get("HostName", "localhost");
            var vPorts = AppConfig.Get("RunningPort", "8100");
            var logger = AppLogger.GetLogger();

            HostControl = hc;
            // listen at multiple ports
            var httpPorts = vPorts.Split(',');
            foreach (var port in httpPorts)
            {
                var baseAddress = string.Format("http://{0}:{1}/", vHostName, port);
                var webApp = WebApp.Start<Startup>(url: baseAddress);
                logger.Info("Open connection to :" + baseAddress);
                _webapps.Add(webApp);
            }

            logger.Info("Webserver started!" + (IsRunningAsConsole ? " (Console Mode)" : string.Empty));
            logger.Info("Voice Cache HashPrefixLength: {0}", VoiceCacheHashPrefixLength);
            return true;
        }

        /// <summary>
        /// Start the web application
        /// </summary>
        public void Start()
        {
            Start(null);
        }

        /// <summary>
        /// Stop the web application
        /// </summary>
        public void Stop()
        {
            foreach (var item in _webapps)
            {
                item?.Dispose();
            }
            _webapps.Clear();
        }
    }
}
