using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Commands;

namespace RecommendMe.Data.CQS.CommandHandlers
{
    public class UpdateRateForArticlesCommandHandler : IRequestHandler<UpdateRateForArticlesCommand>
    {
        private readonly RecommendMeDBContext _dBContext;

        public UpdateRateForArticlesCommandHandler(RecommendMeDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task Handle(UpdateRateForArticlesCommand request, CancellationToken cancellationToken)
        {
            foreach (var kvp in request.Data)
            {
                var article = await _dBContext.Articles.FirstOrDefaultAsync(x => x.Id == kvp.Key, cancellationToken);
                article.PositivityRate = kvp.Value;
            }

            await _dBContext.SaveChangesAsync(cancellationToken);
        }
    }
}
