using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecommendMe.Data;
using RecommendMe.Data.Entities;
using RecommendMe.Services.Abstract;

namespace RecommendMe.Services.Implementation
{
    public class SourceService : ISourceService
    {
        private readonly RecommendMeDBContext _dbContext;
        private readonly ILogger<SourceService> _logger;

        public SourceService(RecommendMeDBContext dbContext, ILogger<SourceService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Source[]> GetSourceWithRss()
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .Where(source => !string.IsNullOrWhiteSpace(source.RssUrl))
                .ToArrayAsync();
        }
    }
}
