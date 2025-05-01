using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace RecommendMe.Services.Mappers
{
    [Mapper]
    public partial class UserMapper
    {
        [MapProperty($"{nameof(Role)}.{nameof(Role.Name)}", nameof(LoginDto.Role))]
        [MapProperty($"{nameof(User.Name)}", nameof(LoginDto.Name))]
        public partial LoginDto UserToLoginDto(User user);



        //[MapProperty($"{nameof(Role)}.{nameof(Role.Name)}", "User")]
        public partial SignInDto UserToSignInDto(User user);
    }
}
