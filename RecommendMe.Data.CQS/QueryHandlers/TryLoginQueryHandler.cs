using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class TryLoginQueryHandler : IRequestHandler<TryLoginQuery, User?>
    {
        private readonly RecommendMeDBContext _context;

        public TryLoginQueryHandler(RecommendMeDBContext context)
        {
            _context = context;
        }

        public async Task<User?> Handle(TryLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(user => user.Role)
                .AsNoTracking()
                .SingleOrDefaultAsync(user => user.Email.Equals(request.Email),
                    cancellationToken);

            return user != null && request.PasswordHash.Equals(user.PasswordHash)
                ? user 
                : null;
        }
    }
}
