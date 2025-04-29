using MediatR;

namespace RecommendMe.Data.CQS.Queries
{
    public class CheckUserWithEmailExistsQuery : IRequest<bool>
    {
        public string Email { get; set; }
    }
}
