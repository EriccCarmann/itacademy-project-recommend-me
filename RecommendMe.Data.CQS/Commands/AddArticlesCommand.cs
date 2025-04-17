using MediatR;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.Commands
{
    public class AddArticlesCommand : IRequest
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}
