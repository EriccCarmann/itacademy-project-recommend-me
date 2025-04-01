using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    public class CommentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
