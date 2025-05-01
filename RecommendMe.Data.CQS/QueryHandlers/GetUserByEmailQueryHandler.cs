using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User>
    {
        private readonly RecommendMeDBContext _dbContext;

        public GetUserByEmailQueryHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Email.Equals(request.Email), cancellationToken);
        }
    }
}
