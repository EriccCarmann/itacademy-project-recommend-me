using System.ServiceModel.Syndication;
using RecommendMe.Services.Abstract;
using System.Xml;
using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Implementation
{
    public class RssService : IRssService
    {
        public async Task<Article[]> GetRssDataAsync(Source rss, CancellationToken token)
        {
            if (string.IsNullOrEmpty(rss.RssUrl)) 
            {
                throw new ArgumentNullException(nameof(rss.RssUrl));
            }

            using (var xmlReader = XmlReader.Create(rss.RssUrl))
            {
                var feed = SyndicationFeed.Load(xmlReader);

                var items = feed.Items
                    .Select(item => new Article
                    {
                        Id = new Guid(),
                        Title = item.Title.Text,
                        Description = item.Summary.Text,
                        CreationDate = item.PublishDate.UtcDateTime,
                        Url = item.Links.FirstOrDefault()?.Uri.ToString(),
                        SourceId = rss.SourceId
                    })
                    .ToArray();

                return items;
            }
        }
    }
}
