using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RecommendMe.Core.DTOs;
using RecommendMe.Services.Abstract;
using RecommendMe.Services.Mappers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecommendMe.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserMapper _userMapper;
        private readonly IMediator _mediator;

        public TokenService(IConfiguration configuration, IMediator mediator, UserMapper userMapper)
        {
            _configuration = configuration;
            _mediator = mediator;
            _userMapper = userMapper;
        }

        public async Task<LoginDto?> TryToRefreshAsync(string requestRefreshToken, CancellationToken cancellationToken = default)
        {
            var userEntity =
                await _mediator.Send(
                    new TryGetUserByRefreshTokenQuery() { RefreshTokenId = Guid.Parse(requestRefreshToken) },
                    cancellationToken);

            await RemoveRefreshTokenAsync(requestRefreshToken);

            return userEntity != null
                ? _userMapper.UserToLoginDto(userEntity)
                : null;
        }

        public async Task RevokeAsync(string requestRefreshToken)
        {
            await _mediator.Send(new RevokeTokenCommand()
            {
                TokenId = Guid.Parse(requestRefreshToken)
            });
        }

        private async Task RemoveRefreshTokenAsync(string requestRefreshToken)
        {
            await _mediator.Send(new RemoveRefreshTokenCommand()
            {
                RefreshToken = Guid.Parse(requestRefreshToken)
            });

        }

        public async Task<(string?, string?)> GenerateTokensPair(LoginDto claimsCollection, CancellationToken cancellationToken = default)
        {
            var claims = new List<Claim>
        {
            //new("Id", claimsCollection.Id.ToString()),
            new(ClaimTypes.Email, claimsCollection.Email),
            new(ClaimTypes.Role, claimsCollection.Role)
        };

            var jwtHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Jwt:Iss"],
                Audience = _configuration["Jwt:Aud"],
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpMinutes"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwtHandler.CreateToken(tokenDescriptor);
            var tokenString = jwtHandler.WriteToken(token);

            var refreshToken = Guid.NewGuid();
            await _mediator.Send(new CreateRefreshTokenCommand()
            {
                RefreshToken = refreshToken,
                UserId = claimsCollection.Id,
                ExpireAt = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration["Jwt:RtExpHours"]))
            }, cancellationToken);

            return (tokenString, refreshToken.ToString("D"));
        }
    }
}
