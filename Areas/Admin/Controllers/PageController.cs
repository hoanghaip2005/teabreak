using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Editor")]
    [Route("Admin/[controller]")]
    public class PageController : Controller
    {
        // GET: /Admin/Page
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý Trang";
            return View();
        }

        // GET: /Admin/Page/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo trang mới";
            return View();
        }

        // GET: /Admin/Page/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Chỉnh sửa trang";
            return View();
        }
    }
} 