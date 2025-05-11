using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class GetArticlesWithoutRateQueryHandler : IRequestHandler<GetArticlesWithoutRateQuery, Article[]>
    {
        private readonly RecommendMeDBContext _dBContext;

        public GetArticlesWithoutRateQueryHandler(RecommendMeDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Article[]> Handle(GetArticlesWithoutRateQuery request, CancellationToken cancellationToken)
        {
            return await _dBContext.Articles
                .AsNoTracking()
                .Include(article => article.Source)
                .Where(article => !article.PositivityRate.HasValue)
                .ToArrayAsync(cancellationToken);
        }
    }
}
