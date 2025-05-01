using MediatR;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Data.CQS.Queries;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Mappers;
using System.Security.Cryptography;
using System.Text;

namespace RecommendMe.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly IMediator _mediator;
        private readonly UserMapper _userMapper;

        public AccountService(IMediator mediator, UserMapper userMapper)
        {
            _mediator = mediator;
            _userMapper = userMapper;
        }

        public async Task<LoginDto> TryToLogin(SignInDto signInDto)
        {
            var passwordHash = GetHash(signInDto.PasswordHash);

            var userDto = _userMapper.UserToSignInDto(await _mediator.Send(new GetUserByEmailQuery()
            {
                Email = signInDto.Email,
            }));

            var result = await _mediator.Send(new TryLoginQuery()
            {
                Name = userDto.Name,
                Email = userDto.Email,
                PasswordHash = userDto.PasswordHash
            });

            return _userMapper.UserToLoginDto(result);
        }

        public async Task<SignInDto?> TryToRegister(RegisterDto registerDro)
        {
            if (await _mediator.Send(new CheckUserWithEmailExistsQuery()
            {
                Email = registerDro.Email
            }))
            {
                return null;
            }

            var passwordHash = GetHash(registerDro.PasswordHash);

            await _mediator.Send(new TryRegisterCommand()
            {
                Name = registerDro.Name,
                Email = registerDro.Email,
                PasswordHash = passwordHash
            });

            var userDto = _userMapper.UserToSignInDto(await _mediator.Send(new GetUserByEmailQuery()
            {
                Email = registerDro.Email,
            }));
            return userDto;
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
