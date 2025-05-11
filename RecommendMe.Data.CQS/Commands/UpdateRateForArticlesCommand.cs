using MediatR;

namespace RecommendMe.Data.CQS.Commands
{
    public class UpdateRateForArticlesCommand : IRequest
    {
        public Dictionary<Guid, double?> Data { get; set; }
    }
}