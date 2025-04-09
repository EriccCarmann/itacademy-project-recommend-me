using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IRssService
    {
        public Task<Article[]> GetRssDataAsync(Source rss, CancellationToken token = default);
    }
}
