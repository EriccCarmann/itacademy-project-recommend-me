using Riok.Mapperly.Abstractions;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Mappers
{
    [Mapper]
    public partial class LoginMapper
    {
        [MapProperty($"{nameof(User.Email)}", nameof(LoginDto.Email))]
        [MapProperty($"{nameof(User.PasswordHash)}", nameof(LoginDto.PasswordHash))]
        public partial LoginDto LoginDtoToUser(User user);
    }
}
