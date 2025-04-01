using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    public class MediaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
