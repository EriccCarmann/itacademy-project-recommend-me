using RecommendMe.Core.DTOs;

namespace RecommendMe.Services.Abstract
{
    public interface IAccountService
    {
        Task<LoginDto> TryToLogin(SignInDto signInDto);
        Task<SignInDto> TryToRegister(RegisterDto registerDro);
        Task CreateRoles();
    }
}
