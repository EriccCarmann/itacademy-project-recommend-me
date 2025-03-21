using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IArticleService
    {
        public Task<Article[]> GetAllPositiveAsync(double posRate, int pageSize, int pageNumber);
        public Task<Article?> GetByIdAsync(int id);
        public Task AddArticleAsync(Article article);
        public Task<int?> CountAsync(double minRate);
    }
}