using Microsoft.AspNetCore.Mvc;

namespace RecommendMe.MVC.Controllers
{
    public class UserController : Controller
    {
        //личный кабинет
        public IActionResult Index()
        {
            return View();
        }
    }
}
