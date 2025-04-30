using Microsoft.AspNetCore.Mvc;
using RecommendMe.Core.DTOs;
using RecommendMe.Data.CQS.Commands;
using RecommendMe.Services.Abstract;

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

        //[HttpGet]
        //public IActionResult Login(RegisterModel)
        //{
        //    return Ok();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            //todo : authentificate user
            var isLoginSucceed = await _accountService.TryToLogin(loginDto);

            if (isLoginSucceed)
            {
                //todo: authorize user
                return Ok();
            }

            return NotFound();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var isLoginSucceed = await _accountService.TryToRegister(registerDto);

            if (isLoginSucceed)
            {
                //todo: authorize user
                return Ok();
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
    }
}
