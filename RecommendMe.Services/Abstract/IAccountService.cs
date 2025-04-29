using RecommendMe.Core.DTOs;

namespace RecommendMe.Services.Abstract
{
    public interface IAccountService
    {
        Task<bool> TryToLogin(LoginDto loginDto);
    }
}
