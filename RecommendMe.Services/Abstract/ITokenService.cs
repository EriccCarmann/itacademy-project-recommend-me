using RecommendMe.Core.DTOs;

namespace RecommendMe.Services.Abstract
{
    public interface ITokenService
    {
        Task<(string?, string?)> GenerateTokensPair(LoginDto claimsCollection, CancellationToken cancellationToken = default);
        Task<LoginDto?> TryToRefreshAsync(string requestRefreshToken, CancellationToken cancellationToken = default);
        Task RevokeAsync(string requestRefreshToken);
    }
}
