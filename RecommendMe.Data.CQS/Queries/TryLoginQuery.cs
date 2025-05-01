using MediatR;
using RecommendMe.Data.Entities;

namespace RecommendMe.Data.CQS.Queries
{
    public class TryLoginQuery : IRequest<User>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
