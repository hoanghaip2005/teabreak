using Microsoft.AspNetCore.Mvc;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class IntroduceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
