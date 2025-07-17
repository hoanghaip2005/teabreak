using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Data;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Toocha;
using App.Areas.Admin.Models;

namespace App.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    [Area("Admin")]
    [Route("/Admin/[action]")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [Route("/Admin")]  // Route đặc biệt cho trang chủ admin
        [Route("/Admin/Home")]
        [Route("/Admin/Home/Index")]
        public async Task<IActionResult> Index()
        {
            var stats = new DashboardStats
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalOrders = await _context.Orders.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalStores = await _context.Stores.CountAsync(),
                TotalUsers = await _context.Users.CountAsync(),
                TodayOrders = await _context.Orders.Where(o => o.OrderDate.Date == DateTime.Today).CountAsync(),
                PendingOrders = await _context.Orders.Where(o => o.Status == OrderStatus.Pending).CountAsync(),
                TotalRevenue = await _context.Orders.Where(o => o.IsPaid).SumAsync(o => o.TotalPrice),
                Stores = await _context.Stores.Where(s => s.IsActive).ToListAsync()
            };

            return View(stats);
        }
    }


} 