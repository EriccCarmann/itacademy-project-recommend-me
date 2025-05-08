using System.ServiceModel.Syndication;
using RecommendMe.Services.Abstract;
using System.Xml;
using RecommendMe.Data.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using RecommendMe.Core.DTOs;
using RecommendMe.Services.Mappers;

namespace RecommendMe.Services.Implementation
{
    public class RssService : IRssService
    {
        private readonly ArticleMapper _articleMapper;

        public RssService(ArticleMapper articleMapper)
        {
            _articleMapper = articleMapper;
        }

        public async Task<Article[]> GetRssDataAsync(string rssUrl, int rssId, CancellationToken token)
        {
            if (string.IsNullOrEmpty(rssUrl)) 
            {
                throw new ArgumentNullException(nameof(rssUrl));
            }

            using (var xmlReader = XmlReader.Create(rssUrl))
            {
                var feed = SyndicationFeed.Load(xmlReader);

                var articles = feed.Items
                    .Select(item => GetArticleFromSyndicationItem(item, rssId))
                    //.Select(article => _articleMapper.ArticleToArticleDto(article))
                    .ToArray();

                return articles;
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
