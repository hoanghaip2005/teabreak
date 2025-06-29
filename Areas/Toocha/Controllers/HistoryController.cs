using Microsoft.AspNetCore.Mvc;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult SuMenh()
        {
            return View();
        }
    }
} 