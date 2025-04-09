using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IArticleService
    {
        //public Task DeleteAll();
        public Task<Article[]> GetAllPositiveAsync(double posRate, int pageSize, int pageNumber, CancellationToken token = default);
        public Task<Article?> GetByIdAsync(int id, CancellationToken token = default);
        public Task AddArticleAsync(Article article, CancellationToken token = default);
        public Task<int?> CountAsync(double minRate, CancellationToken token = default);

        public Task<string[]> GetUniqueArticlesUrls(CancellationToken token = default);
        public Task AddArticlesAsync(IEnumerable<Article> newUniqueArticles, CancellationToken token = default);
        public Task<Guid[]> GetArticleIdsWithNoTextAsync(CancellationToken token = default);
        public Task UpdateContentByWebScrapping(Guid[] ids, CancellationToken token = default);
    }
}