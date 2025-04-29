using RecommendMe.Core.DTOs;
using RecommendMe.Data.Entities;
using Riok.Mapperly.Abstractions;

namespace RecommendMe.Services.Mappers
{
    [Mapper]
    public partial class RegistrationMapper
    {
        [MapProperty($"{nameof(User.Name)}", nameof(RegisterDto.Name))]
        [MapProperty($"{nameof(User.Email)}", nameof(RegisterDto.Email))]
        [MapProperty($"{nameof(User.PasswordHash)}", nameof(RegisterDto.PasswordHash))]
        public partial RegisterDto RegisterDtoToUser(User user);
    }
}
