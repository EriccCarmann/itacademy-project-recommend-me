using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    public class SourceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
