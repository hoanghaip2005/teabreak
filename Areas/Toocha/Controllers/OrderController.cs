using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using toocha.Models;

namespace toocha.Areas.Toocha.Controllers;

[Area("Toocha")]
public class OrderController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
