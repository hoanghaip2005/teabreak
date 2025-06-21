using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Toocha;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    [Authorize(Roles = $"{RoleName.Administrator},{RoleName.Seller},{RoleName.Manager}")]
    public class SellerController : Controller
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var thisWeek = today.AddDays(-(int)today.DayOfWeek);
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            var dashboardData = new SellerDashboardViewModel
            {
                TodayOrders = await _context.Orders.CountAsync(o => o.OrderDate.Date == today),
                TodayRevenue = await _context.Orders
                    .Where(o => o.OrderDate.Date == today && o.IsPaid)
                    .SumAsync(o => o.TotalPrice),
                WeekOrders = await _context.Orders.CountAsync(o => o.OrderDate >= thisWeek),
                WeekRevenue = await _context.Orders
                    .Where(o => o.OrderDate >= thisWeek && o.IsPaid)
                    .SumAsync(o => o.TotalPrice),
                MonthOrders = await _context.Orders.CountAsync(o => o.OrderDate >= thisMonth),
                MonthRevenue = await _context.Orders
                    .Where(o => o.OrderDate >= thisMonth && o.IsPaid)
                    .SumAsync(o => o.TotalPrice),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
                ProcessingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Processing),
                ShippedOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Shipped),
                
                // Đơn hàng gần đây
                RecentOrders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(10)
                    .ToListAsync(),

                // Top sản phẩm bán chạy
                TopProducts = await _context.OrderItems
                    .Include(oi => oi.Product)
                    .Where(oi => oi.Order.OrderDate >= thisMonth)
                    .GroupBy(oi => oi.Product)
                    .Select(g => new TopProductViewModel
                    {
                        Product = g.Key,
                        TotalQuantity = g.Sum(oi => oi.Quantity),
                        TotalRevenue = g.Sum(oi => oi.ItemPrice)
                    })
                    .OrderByDescending(tp => tp.TotalQuantity)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboardData);
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenueChart(int days = 7)
        {
            var startDate = DateTime.Today.AddDays(-days);
            var endDate = DateTime.Today;

            var revenueData = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.IsPaid)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalPrice),
                    Orders = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            // Đảm bảo có dữ liệu cho tất cả các ngày trong khoảng thời gian
            var allDates = Enumerable.Range(0, days + 1)
                .Select(i => startDate.AddDays(i))
                .ToList();

            var result = allDates.Select(date => new
            {
                Date = date.ToString("dd/MM"),
                Revenue = revenueData.FirstOrDefault(r => r.Date == date)?.Revenue ?? 0,
                Orders = revenueData.FirstOrDefault(r => r.Date == date)?.Orders ?? 0
            }).ToList();

            return Json(result);
        }
    }

    public class SellerDashboardViewModel
    {
        public int TodayOrders { get; set; }
        public decimal TodayRevenue { get; set; }
        public int WeekOrders { get; set; }
        public decimal WeekRevenue { get; set; }
        public int MonthOrders { get; set; }
        public decimal MonthRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippedOrders { get; set; }
        public List<Order> RecentOrders { get; set; } = new();
        public List<TopProductViewModel> TopProducts { get; set; } = new();
    }

    public class TopProductViewModel
    {
        public Product Product { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }
} 