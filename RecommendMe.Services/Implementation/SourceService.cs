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

        public async Task<Source> GetByIdAsync(int id, CancellationToken token = default)
        {
            return await _dbContext.Sources
                 .AsNoTracking()
                 .FirstOrDefaultAsync(source => source.SourceId.Equals(id), token);
        }

        public async Task<Source[]> GetSourceWithRss(CancellationToken token = default)
        {
            return await _dbContext.Sources
                .AsNoTracking()
                .Where(source => !string.IsNullOrWhiteSpace(source.RssUrl))
                .ToArrayAsync(token);
        }
    }
}
