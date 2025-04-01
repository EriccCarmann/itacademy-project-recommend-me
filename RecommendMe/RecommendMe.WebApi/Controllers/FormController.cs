using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.WebApi.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
