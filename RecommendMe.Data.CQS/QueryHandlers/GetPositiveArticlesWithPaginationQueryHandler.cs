using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class GetPositiveArticlesWithPaginationQueryHandler : IRequestHandler<GetPositiveArticlesWithPaginationQuery, Article[]>
    {
        private readonly RecommendMeDBContext _dbContext;

        public GetPositiveArticlesWithPaginationQueryHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Article[]> Handle(GetPositiveArticlesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Articles
               .Where(article => article.PositivityRate >= request.PositivityRate || !article.PositivityRate.HasValue)
               .Include(article => article.Source)
               .OrderByDescending(article => article.CreationDate)
               .AsNoTracking()
               .Skip((request.Page - 1) * request.PageSize)
               .Take(request.PageSize)
               .ToArrayAsync(cancellationToken);

            return result;
        }
    }
}
