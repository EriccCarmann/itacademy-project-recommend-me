using MediatR;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.Queries
{
    public class GetPositiveArticlesWithPaginationQuery : IRequest<Article>
    {
        public int Page {  get; set; }
        public int PageSize {  get; set; }
        public double? PositivityRate { get; set; }
    }
}
