using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        [HttpGet("index")]
        public async Task<IActionResult> Index()
        {
            return Ok();
        }
    }
}
