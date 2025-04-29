using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class CheckUserWithEmailExistsQueryHandler : IRequestHandler<CheckUserWithEmailExistsQuery, bool>
    {
        private readonly RecommendMeDBContext _dbContext;

        public CheckUserWithEmailExistsQueryHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CheckUserWithEmailExistsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(user => user.Email.Equals(request.Email), cancellationToken);
        }
    }
}
