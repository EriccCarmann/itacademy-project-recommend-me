using MediatR;

namespace RecommendMe.Data.CQS.Queries
{
    public class GetIdsForArticlesWithNoTextQuery : IRequest<Guid[]> { }
}
