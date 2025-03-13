using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IArticleService
    {
        public Task<Article[]> GetAllPositiveAsync(double posRate);
        public Task<Article?> GetByIdAsync(int id);
        public Task AddArticleAsync(Article article);
    }
}