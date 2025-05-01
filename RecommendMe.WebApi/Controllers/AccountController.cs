using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Services.Abstract;
using System.Security.Claims;

namespace RecommendMe.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SignInDto signInDto)
        {
            var loginData = await _accountService.TryToLogin(signInDto);

            if (loginData != null)
            {
                await SignIn(loginData);
                return Ok();
            }

            return NotFound();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var loginDto = await _accountService.TryToRegister(registerDto);

            if (loginDto != null)
            {
                await Login(loginDto);
            }

            return NotFound();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        [HttpPost("createroles")]
        public Task CreateRoles()
        {
            return _accountService.CreateRoles();
        }

        private async Task SignIn(LoginDto loginData)
        {
            var claims = new List<Claim>
                {
                    new (ClaimTypes.Email, loginData.Email),
                    new (ClaimTypes.Role, loginData.Role),
                    new (ClaimTypes.Name, loginData.Name)
                };

            var claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
        }

    }
}
