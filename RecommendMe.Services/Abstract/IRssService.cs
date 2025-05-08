using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IRssService
    {
        public Task<Article[]> GetRssDataAsync(string rssUrl, int rssId, CancellationToken token = default);
    }
}
