using Riok.Mapperly.Abstractions;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;

namespace RecommendMe.Services.Mappers
{
    [Mapper]
    public partial class LoginMapper
    {
        [MapProperty($"{nameof(User.Name)}", nameof(LoginDto.Name))]
        [MapProperty($"{nameof(User.Email)}", nameof(LoginDto.Email))]
        [MapProperty($"{nameof(User.PasswordHash)}", nameof(LoginDto.PasswordHash))]
        //[MapProperty($"{nameof(User.RoleId)}", "2")]
        //[MapProperty($"{nameof(User.Role)}", "User")]
        public partial LoginDto LoginDtoToUser(User user);
    }
}
