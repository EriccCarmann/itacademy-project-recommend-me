using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Commands;

namespace RecommendMe.Data.CQS.CommandHandlers
{
    public class UpdateTextForArticlesCommandHandler : IRequestHandler<UpdateTextForArticlesCommand>
    {
        private readonly RecommendMeDBContext _dbContext;

        public UpdateTextForArticlesCommandHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateTextForArticlesCommand request, CancellationToken cancellationToken)
        {
            foreach (var keyValuePair in request.Data)
            {
                var article = await _dbContext.Articles.SingleOrDefaultAsync(article => article.Id.Equals(keyValuePair.Key), cancellationToken);

                if (article != null)
                {
                    article.Content = keyValuePair.Value;
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
