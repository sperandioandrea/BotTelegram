using System;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

public static class RssService
{
    public static async Task<string> GetRssFeedContent(string feedUrl)
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetStringAsync(feedUrl);

            var settings = new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse  
            };

            using (var stringReader = new System.IO.StringReader(response))
            using (var xmlReader = XmlReader.Create(stringReader, settings)) 
            {
                var feed = SyndicationFeed.Load(xmlReader);
                var feedContent = string.Join("\n\n", feed.Items.Take(5).Select(item => $"{item.Title.Text}\n{item.Summary.Text}"));
                return feedContent;
            }
        }
    }
}