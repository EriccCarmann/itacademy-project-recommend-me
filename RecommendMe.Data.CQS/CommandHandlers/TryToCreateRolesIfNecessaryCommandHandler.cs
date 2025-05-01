using MediatR;
using Microsoft.EntityFrameworkCore;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.CommandHandlers
{
    public class TryToCreateRolesIfNecessaryCommandHandler : IRequestHandler<TryToCreateRolesIfNecessaryCommand>
    {
        private readonly RecommendMeDBContext _dbContext;

        public TryToCreateRolesIfNecessaryCommandHandler(RecommendMeDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Handle(TryToCreateRolesIfNecessaryCommand request, CancellationToken cancellationToken)
        {
            string[] necessaryRoles = ["User", "Admin"];

            var existedRoles = await _dbContext.Roles
                .AsNoTracking()
                .Where(r => necessaryRoles.Contains(r.Name))
                .Select(r => r.Name)
                .ToArrayAsync(cancellationToken);

            necessaryRoles = necessaryRoles.Except(existedRoles).ToArray();

            foreach (var role in necessaryRoles)
            {
                if (!_dbContext.Roles.Any(r => r.Name == role))
                {
                    _dbContext.Roles.Add(new Role
                    {
                        Name = role
                    });
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
