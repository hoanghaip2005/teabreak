using Microsoft.AspNetCore.Mvc;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}