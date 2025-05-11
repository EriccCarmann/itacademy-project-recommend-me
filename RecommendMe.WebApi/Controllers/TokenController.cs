using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RecommendMe.Services.Abstract;

namespace RecommendMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IAccountService _accountService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ITokenService tokenService, IAccountService accountService, ILogger<TokenController> logger)
        {
            _tokenService = tokenService;
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var loginDto = await _accountService.TryToLoginAsync(request.Email, request.Password);
            if (loginDto == null)
            {
                return Unauthorized(new { Message = "Incorrect username or password" });
            }
            var tokensPair = await _tokenService.GenerateTokensPair(loginDto);
            if (tokensPair.Item1 == null || tokensPair.Item2 == null)
            {
                return BadRequest();
            }
            return Ok(new TokenPairResponse()
            {
                Jwt = tokensPair.Item1,
                RefreshToken = tokensPair.Item2
            });
        }

        [HttpPost]
        [Route("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var loginDto = await _tokenService.TryToRefreshAsync(request.RefreshToken);
            if (loginDto == null)
            {
                return Unauthorized(new { Message = "Refresh token is not active." });
            }
            var tokensPair = await _tokenService.GenerateTokensPair(loginDto);

            if (tokensPair.Item1 == null || tokensPair.Item2 == null)
            {
                return BadRequest();
            }
            return Ok(new TokenPairResponse()
            {
                Jwt = tokensPair.Item1,
                RefreshToken = tokensPair.Item2
            });
        }

        [HttpPost]
        [Route("Revoke")] // This endpoint is used to revoke the refresh token, should be changes with removing
        [Authorize]
        public async Task<IActionResult> Revoke([FromBody] RefreshTokenRequest request)
        {
            await _tokenService.RevokeAsync(request.RefreshToken);
            return Ok();
        }
    }
}
