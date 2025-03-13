using Microsoft.EntityFrameworkCore;
using RecommendMe.Data;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;

namespace RecommendMe.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly RecommendMeDBContext _dbContext;

        public ArticleService(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article[]> GetAllPositiveAsync(double minRate)
        {
            return await _dbContext.Articles
                .Where(article => article.PositivityRate >= minRate)
                .Include(article => article.Source)
                .AsNoTracking()
                .ToArrayAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _dbContext.Articles
                .FirstOrDefaultAsync(article => article.Id.Equals(id));
        }

        public async Task AddArticleAsync(Article article)
        {
            await _dbContext.Articles.AddAsync(article);
            await _dbContext.SaveChangesAsync();
        }
    }
}
