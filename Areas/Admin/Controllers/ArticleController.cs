using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Editor")]
    [Route("Admin/[controller]")]
    public class ArticleController : Controller
    {
        // GET: /Admin/Article
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Danh sách bài viết";
            return View();
        }

        // GET: /Admin/Article/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo bài viết mới";
            return View();
        }

        // GET: /Admin/Article/Categories
        [HttpGet("Categories")]
        public IActionResult Categories()
        {
            ViewData["Title"] = "Danh mục bài viết";
            return View();
        }

        // GET: /Admin/Article/Groups
        [HttpGet("Groups")]
        public IActionResult Groups()
        {
            ViewData["Title"] = "Nhóm bài viết";
            return View();
        }

        // GET: /Admin/Article/Edit/{id}
        [HttpGet("Edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Chỉnh sửa bài viết";
            return View();
        }
    }
} 