using System.ServiceModel.Syndication;
using RecommendMe.Services.Abstract;
using System.Xml;
using RecommendMe.Data.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

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
                    .Select(item => GetArticleFromSyndicationItem(item, rss.SourceId))
                    .ToArray();

                return items;
            }
        }

        private Article GetArticleFromSyndicationItem(SyndicationItem item, int sourceId)
        {
            var (imgUrl, content) = GetImageUrlAndContent(item);
            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = item.Title.Text,
                Description = content,
                CreationDate = item.PublishDate.UtcDateTime,
                Url = item.Id,
                ImageUrl = imgUrl,
                SourceId = sourceId
            };

            return article;
        }

        private (string, string) GetImageUrlAndContent(SyndicationItem item)
        {
            var content = item.Summary;
            var imageUrl = string.Empty;
            var text = new StringBuilder();

            if (content != null)
            {
                var match = Regex.Match(content.Text, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    imageUrl = match.Groups[1].Value;
                }
                var textMatches = Regex.Matches(content.Text, @"<p>(?:(?!<\/p>).)*<\/p>", RegexOptions.Singleline);

                foreach (Match textMatch in textMatches.SkipLast(1).Reverse().SkipLast(1).Reverse())
                {
                    text.AppendLine(textMatch.Value);
                }
            }
            return (imageUrl, text.ToString());
        }
    }
}
