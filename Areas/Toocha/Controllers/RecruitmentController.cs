using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Toocha;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class RecruitmentController : Controller
    {
        private readonly AppDbContext _context;

        public RecruitmentController(AppDbContext context)
        {
            _context = context;
        }

        [Route("toocha/recruitment")]
        [Route("toocha/recruitment/index")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Về Chúng Tôi - Tocha";
            return View();
        }

        [Route("toocha/recruitment/positions")]
        public IActionResult Positions()
        {
            ViewData["Title"] = "Các Vị Trí Tuyển Dụng - Tocha";
            // Có thể mở rộng sau để hiển thị danh sách các vị trí tuyển dụng
            return View();
        }

        // API endpoint để lấy danh sách các vị trí tuyển dụng (có thể mở rộng sau)
        [HttpGet]
        public async Task<IActionResult> GetJobPositions()
        {
            // Placeholder - có thể mở rộng sau khi có model JobPosition
            var positions = new[]
            {
                new { Id = 1, Title = "Nhân viên pha chế", Department = "Cửa hàng", Location = "Hà Nội" },
                new { Id = 2, Title = "Quản lý ca", Department = "Cửa hàng", Location = "TP.HCM" },
                new { Id = 3, Title = "Marketing Executive", Department = "Marketing", Location = "Hà Nội" }
            };

            return Json(positions);
        }
    }
} 