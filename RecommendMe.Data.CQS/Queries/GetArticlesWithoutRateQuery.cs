using MediatR;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.Queries
{
    public class GetArticlesWithoutRateQuery : IRequest<Article[]> { }
}
