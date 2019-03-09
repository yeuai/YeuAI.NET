using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;

namespace YeuAI.Spider.Entities
{
    [Schema("baidu", "baidu_search_entity_model")]
    [Entity(Expression = ".//div[@class='result']", Type = SelectorType.XPath)]
    public class BaiduSearchEntry : BaseEntity
    {
        [Column]
        [Field(Expression = "Keyword", Type = SelectorType.Enviroment)]
        public string Keyword { get; set; }

        [Column]
        [Field(Expression = ".//h3[@class='c-title']/a")]
        [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
        [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
        public string Title { get; set; }

        [Column]
        [Field(Expression = ".//h3[@class='c-title']/a/@href")]
        public string Url { get; set; }

        [Column]
        [Field(Expression = ".//div/p[@class='c-author']/text()")]
        [ReplaceFormatter(NewValue = "-", OldValue = "&nbsp;")]
        public string Website { get; set; }

        [Column]
        [Field(Expression = ".//div/span/a[@class='c-cache']/@href")]
        public string Snapshot { get; set; }

        [Column]
        [Field(Expression = ".//div[@class='c-summary c-row ']", Option = FieldOptions.InnerText)]
        [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
        [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
        [ReplaceFormatter(NewValue = " ", OldValue = "&nbsp;")]
        public string Details { get; set; }

        [Column(Length = 0)]
        [Field(Expression = ".", Option = FieldOptions.InnerText)]
        [ReplaceFormatter(NewValue = "", OldValue = "<em>")]
        [ReplaceFormatter(NewValue = "", OldValue = "</em>")]
        [ReplaceFormatter(NewValue = " ", OldValue = "&nbsp;")]
        public string PlainText { get; set; }
    }
}
