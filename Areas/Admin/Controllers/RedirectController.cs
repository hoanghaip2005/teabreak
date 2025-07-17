using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Editor")]
    [Route("Admin/[controller]")]
    public class RedirectController : Controller
    {
        // GET: /Admin/Redirect
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý Redirect";
            return View();
        }

        // GET: /Admin/Redirect/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo redirect mới";
            return View();
        }

        // GET: /Admin/Redirect/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Chỉnh sửa redirect";
            return View();
        }
    }
} 