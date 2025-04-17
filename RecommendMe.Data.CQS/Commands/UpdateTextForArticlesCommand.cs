using MediatR;

namespace RecommendMe.Data.CQS.Commands
{
    public class UpdateTextForArticlesCommand : IRequest
    {
        public Dictionary<Guid, string> Data { get; set; }
    }
}
