using MediatR;

namespace RecommendMe.Data.CQS.Commands
{
    public class TryRegisterCommand : IRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
