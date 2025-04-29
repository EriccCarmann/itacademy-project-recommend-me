using MediatR;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Services.Abstract;

namespace RecommendMe.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IMediator _mediator;

        public AccountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> TryToLogin(LoginDto loginDto)
        {
            var result = await _mediator.Send(new TryLoginQuery()
            {
                Name = loginDto.Name,
                Email = loginDto.Email,
                PasswordHash = loginDto.PasswordHash
            });

            return result;
        }
    }
}
