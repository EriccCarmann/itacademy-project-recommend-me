using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Queries;

namespace RecommendMe.Data.CQS.QueryHandlers
{
    public class TryLoginQueryHandler : IRequestHandler<TryLoginQuery, bool>
    {
        private readonly RecommendMeDBContext _context;

        public TryLoginQueryHandler(RecommendMeDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(TryLoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email.Equals(request.Email), 
                cancellationToken);

            return user != null && request.PasswordHash.Equals(user.PasswordHash);
        }
    }
}
