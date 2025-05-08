using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IRssService
    {
        public Task<ArticleDto[]> GetRssDataAsync(string rssUrl, int rssId, CancellationToken token = default);
    }
}
