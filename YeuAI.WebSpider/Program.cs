
using DotnetSpider;
using System;
using YeuAI.WebSpider.Spiders;

namespace YeuAI.WebSpider
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var spider = Spider.Create<GithubSpider>();
            spider.RunAsync();
            Console.Read();
        }
    }
}
