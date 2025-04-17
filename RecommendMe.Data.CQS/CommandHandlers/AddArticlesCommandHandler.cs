using MediatR;
using RecommendMe.Data.CQS.Commands;

namespace RecommendMe.Data.CQS.CommandHandlers
{
    public class AddArticlesCommandHandler : IRequestHandler<AddArticlesCommand>
    {
        private readonly RecommendMeDBContext _dbContext;

        public AddArticlesCommandHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(AddArticlesCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.AddRangeAsync(request.Articles, cancellationToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}
