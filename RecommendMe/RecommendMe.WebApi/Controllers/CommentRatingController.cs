using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    public class CommentRatingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
