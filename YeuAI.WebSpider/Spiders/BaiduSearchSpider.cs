using DotnetSpider.Extension;
using DotnetSpider.Extension.Pipeline;
using System.Collections.Generic;
using YeuAI.Spider.Entities;

namespace YeuAI.Spider.Spiders
{
    public class BaiduSearchSpider : EntitySpider
    {
        protected override void OnInit(params string[] arguments)
        {
            var word = "可乐|雪碧";
            AddRequest(string.Format("http://news.baidu.com/ns?word={0}&tn=news&from=news&cl=2&pn=0&rn=20&ct=1", word), new Dictionary<string, dynamic> { { "Keyword", word } });
            AddEntityType<BaiduSearchEntry>();
            AddPipeline(new ConsoleEntityPipeline());
        }
    }
}
