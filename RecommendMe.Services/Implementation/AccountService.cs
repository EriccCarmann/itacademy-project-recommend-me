using MediatR;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Services.Abstract;
using System.Security.Cryptography;
using System.Text;

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
                Email = loginDto.Email,
                PasswordHash = loginDto.PasswordHash
            });

            return result;
        }

        public async Task<bool> TryToRegister(RegisterDto registerDro)
        {
            if (await _mediator.Send(new CheckUserWithEmailExistsQuery()
            {
                Email = registerDro.Email
            }))
            {
                return false;
            }

            var passwordHash = GetHash(registerDro.PasswordHash);

            await _mediator.Send(new TryRegisterCommand()
            {
                Name = registerDro.Name,
                Email = registerDro.Email,
                PasswordHash = passwordHash
            });

            return true;
        }

        public string GetHash(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }

        public Task CreateRoles()
        {
            return _mediator.Send(new TryToCreateRolesIfNecessaryCommand());
        }
    }
}
