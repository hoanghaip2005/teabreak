using Microsoft.AspNetCore.Mvc;

namespace Toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class FranchiseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
