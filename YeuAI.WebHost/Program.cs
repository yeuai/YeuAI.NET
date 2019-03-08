using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace YeuAI.WebHost
{
    class Program
    {
        /// <summary>
        /// Start the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            StartTopshelf();
        }

        /// <summary>
        /// Topshelf - Application startup supporter
        /// </summary>
        static void StartTopshelf()
        {
            HostFactory.Run(x =>
            {
                x.Service<WebServer>(s =>
                {
                    s.ConstructUsing(name => new WebServer());
                    s.WhenStarted((tc, hc) => tc.Start(hc));
                    s.WhenStopped(tc => tc.Stop());
                });
                x.UseNLog();
                x.RunAsLocalSystem();

                x.SetServiceName("YeuAI.WebHost");
                x.SetDisplayName("YeuAI.WebHost - YeuAI Cloud API Service");
                x.SetDescription("YeuAI.WebHost - YeuAI Artificial Intelligence");
            });
        }
    }
}
