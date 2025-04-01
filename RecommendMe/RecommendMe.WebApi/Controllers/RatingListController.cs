using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    public class RatingListController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
