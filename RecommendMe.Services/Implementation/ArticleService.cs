using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecommendMe.Data;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;

namespace RecommendMe.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly RecommendMeDBContext _dbContext;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(RecommendMeDBContext dbContext, ILogger<ArticleService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Article[]> GetAllPositiveAsync(double minRate, int pageSize, int pageNumber, CancellationToken token = default)
        {
            return await _dbContext.Articles
                .Where(article => article.PositivityRate >= minRate)
                .Include(article => article.Source)
                .AsNoTracking()
                .OrderByDescending(article => article.PositivityRate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync(token);
        }

        public async Task<Article?> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Include(article => article.Source)
                .FirstOrDefaultAsync(article => article.Id.Equals(id), token);
        }

        public async Task AddArticleAsync(Article article, CancellationToken token = default)
        {
            await _dbContext.Articles.AddAsync(article);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<int?> CountAsync(double minRate, CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .CountAsync(token);
        }

        public async Task<string[]> GetUniqueArticlesUrls(CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Select(article => article.Url)
                .Distinct()
                .ToArrayAsync(token);
        }

        public async Task AddArticlesAsync(IEnumerable<Article> newUniqueArticles, CancellationToken token = default)
        {
            await _dbContext.AddRangeAsync(newUniqueArticles, token);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<Guid[]> GetArticleIdsWithNoTextAsync(CancellationToken token = default)
        {
            return await _dbContext.Articles
                .AsNoTracking()
                .Where(article => !string.IsNullOrWhiteSpace(article.Url) 
                                  && string.IsNullOrWhiteSpace(article.Content))
                .Select(article => article.Id)
                .ToArrayAsync(token);
        }

        public async Task UpdateContentByWebScrapping(Guid[] ids, CancellationToken token = default)
        {
            foreach (Guid id in ids)
            {
                var article = await _dbContext.Articles.FirstOrDefaultAsync(article => article.Id.Equals(id), token);
                if (article != null || string.IsNullOrWhiteSpace(article.Url)) 
                {
                    _logger.LogWarning($"Article with {id} not found or has no url", id);
                    continue;
                }

                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(article.Url, token);

                if (doc == null)
                {
                    _logger.LogWarning($"Failed to load article from {article.Url}", article.Url);
                    continue;
                }

                //var articleMode
            }
        }
    }
}
