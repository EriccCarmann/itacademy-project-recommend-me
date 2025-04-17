using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class GetIdsForArticlesWithNoTextQueryHandler : IRequestHandler<GetIdsForArticlesWithNoTextQuery, Guid[]>
    {
        private readonly RecommendMeDBContext _dbContext;

        public GetIdsForArticlesWithNoTextQueryHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid[]> Handle(GetIdsForArticlesWithNoTextQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Articles
                    .AsNoTracking()
                    .Where(article => !string.IsNullOrWhiteSpace(article.Url)
                                      && string.IsNullOrWhiteSpace(article.Content))
                    .Select(article => article.Id)
                    .ToArrayAsync(cancellationToken);
        }
    }
}
