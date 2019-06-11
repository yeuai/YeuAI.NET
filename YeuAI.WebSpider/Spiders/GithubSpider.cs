using System;
using System.Threading.Tasks;
using DotnetSpider;
using DotnetSpider.Core;
using DotnetSpider.Data;
using DotnetSpider.Data.Parser;
using DotnetSpider.Data.Storage;
using DotnetSpider.Downloader;
using DotnetSpider.MessageQueue;
using DotnetSpider.Statistics;
using Microsoft.Extensions.Logging;

namespace YeuAI.WebSpider.Spiders
{
    public class GithubSpider : Spider
    {
        class Parser : DataParserBase
        {
            protected override Task<DataFlowResult> Parse(DataFlowContext context)
            {
                var selectable = context.GetSelectable();
                // Parsing data
                var author = selectable.XPath("//span[@class='p-name vcard-fullname d-block overflow-hidden']")
                    .GetValue();
                var name = selectable.XPath("//span[@class='p-nickname vcard-username d-block']")
                    .GetValue();
                context.AddItem("author", author);
                context.AddItem("username", name);

                // Add target link
                var urls = selectable.Links().Regex("(https://github\\.com/[\\w\\-]+/[\\w\\-]+)").GetValues();
                AddTargetRequests(context, urls);

                // If the parsing is empty, skip the next step
                if (string.IsNullOrWhiteSpace(name))
                {
                    context.ClearItems();
                    return Task.FromResult(DataFlowResult.Terminated);
                }

                return Task.FromResult(DataFlowResult.Success);
            }
        }

        public GithubSpider(IMessageQueue mq, IStatisticsService statisticsService, ISpiderOptions options,
            ILogger<Spider> logger, IServiceProvider services) : base(mq, statisticsService, options, logger, services)
        {
        }

        protected override void Initialize()
        {
            NewGuidId();
            RetryDownloadTimes = 3;
            DownloaderSettings.Timeout = 5000;
            DownloaderSettings.Type = DownloaderType.HttpClient;
            AddDataFlow(new Parser());
            AddDataFlow(new ConsoleStorage());
            AddRequests("https://github.com/vunb");
        }
    }
}

