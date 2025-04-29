using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.CommandHandlers
{
    public class TryRegisterCommandHandler : IRequestHandler<TryRegisterCommand>
    {
        private readonly RecommendMeDBContext _dbContext;

        public TryRegisterCommandHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(TryRegisterCommand request, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles
                .AsNoTracking()
                .SingleOrDefaultAsync(role => role.Name.Equals("User"), cancellationToken: cancellationToken);

            if (role != null)
            {
                await _dbContext.Users.AddAsync(new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = request.PasswordHash,
                    RoleId = role.Id
                });

                _dbContext.SaveChanges();
            }
        }
    }
}
