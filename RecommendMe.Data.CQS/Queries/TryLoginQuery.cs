using MediatR;

namespace RecommendMe.Data.CQS.Queries
{
    public class TryLoginQuery : IRequest<bool>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
