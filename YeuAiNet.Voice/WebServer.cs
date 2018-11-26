using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Topshelf.Hosts;
using YeuAiNet.Common;
using YeuAiNet.Common.Logging;

namespace YeuAiNet.Voice
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
            var vHostName = AppConfiguration.Get("HostName", "localhost");
            var vPorts = AppConfiguration.Get("RunningPort", "8100");
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
            
            logger.Info("Webserver started!" + (IsRunningAsConsole ? " (Console Mode)": string.Empty));
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
