using System;
using Topshelf;

namespace YeuAiNet.Voice
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
        /// + Background console: EHR.QLID.exe run
        /// + Service register: EHR.QLID.exe install
        /// + Service startup: EHR.QLID.exe start/stop
        /// + Service remove: EHR.QLID.exe uninstall
        /// + Other help: EHR.QLID.exe help
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

                x.SetServiceName("YeuAiNet.Voice");
                x.SetDisplayName("YeuAiNet.Voice - YeuAI Voice Api");
                x.SetDescription("YeuAiNet.Voice - YeuAI Artificial Intelligence");
            });
        }
    }
}
