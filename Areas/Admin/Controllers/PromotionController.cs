using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator,Editor")]
    [Route("Admin/[controller]")]
    public class PromotionController : Controller
    {
        private readonly AppDbContext _context;

        public PromotionController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Promotion
        [HttpGet("")]
        public IActionResult Index()
        {
            ViewData["Title"] = "Quản lý Khuyến mãi";
            return View();
        }

        // GET: /Admin/Promotion/Coupons
        [HttpGet("Coupons")]
        public async Task<IActionResult> Coupons()
        {
            ViewData["Title"] = "Mã giảm giá";
            
            // Fetch discounts from database
            var discounts = await _context.Discounts
                .Include(d => d.Product)
                .Include(d => d.Category)
                .OrderByDescending(d => d.Id)
                .ToListAsync();

            // Calculate statistics
            var activeDiscounts = discounts.Count(d => d.IsActive && d.EndDate > DateTime.Now && !d.IsLimitReached);
            var totalUsages = discounts.Sum(d => d.UsedCount);
            var expiredDiscounts = discounts.Count(d => d.EndDate <= DateTime.Now);
            var expiringSoon = discounts.Count(d => d.IsActive && d.EndDate > DateTime.Now && d.EndDate <= DateTime.Now.AddDays(7));
            
            ViewBag.ActiveDiscounts = activeDiscounts;
            ViewBag.TotalUsages = totalUsages;
            ViewBag.ExpiredDiscounts = expiredDiscounts;
            ViewBag.ExpiringSoon = expiringSoon;
            ViewBag.TotalDiscounts = discounts.Count;

            return View(discounts);
        }

        // GET: /Admin/Promotion/Programs
        [HttpGet("Programs")]
        public IActionResult Programs()
        {
            ViewData["Title"] = "Chương trình khuyến mãi";
            return View();
        }

        // GET: /Admin/Promotion/Gifts
        [HttpGet("Gifts")]
        public IActionResult Gifts()
        {
            ViewData["Title"] = "Chương trình tặng kèm";
            return View();
        }

        // GET: /Admin/Promotion/Create
        [HttpGet("Create")]
        public IActionResult Create()
        {
            ViewData["Title"] = "Tạo khuyến mãi mới";
            return View();
        }
    }
} 