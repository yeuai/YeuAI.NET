using YeuAI.Spider.Spiders;

namespace YeuAI.WebSpider
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var spider = new BaiduSearchSpider();
            spider.Run();
        }
    }
}
