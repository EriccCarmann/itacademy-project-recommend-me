using ReccomendMe.Data.Entities;

namespace RecommendMe.Services.Abstract
{
    public interface IArticleService
    {
        public Task<Article[]> GetAllPositiveAsync(double posRate);
        public Task<Article?> GetByIdAsync(Guid id);
    }
}