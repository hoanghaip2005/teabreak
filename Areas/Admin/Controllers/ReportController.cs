using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Toocha;

namespace App.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    [Area("Admin")]
    [Route("/Admin/Report/[action]")]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;

        public ReportController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Report/Revenue
        [Route("/Admin/Report")]
        public async Task<IActionResult> Revenue(DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Set default date range (last 30 days)
            fromDate ??= DateTime.Today.AddDays(-30);
            toDate ??= DateTime.Today;

            var query = _context.Orders
                .Where(o => o.OrderDate.Date >= fromDate.Value.Date && o.OrderDate.Date <= toDate.Value.Date);

            var revenueData = new RevenueReportViewModel
            {
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                TotalOrders = await query.CountAsync(),
                TotalRevenue = await query.Where(o => o.IsPaid).SumAsync(o => o.TotalPrice),
                PaidOrders = await query.Where(o => o.IsPaid).CountAsync(),
                UnpaidOrders = await query.Where(o => !o.IsPaid).CountAsync(),
                DailyRevenue = await query
                    .Where(o => o.IsPaid)
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new DailyRevenueItem
                    {
                        Date = g.Key,
                        Revenue = g.Sum(o => o.TotalPrice),
                        OrderCount = g.Count()
                    })
                    .OrderBy(x => x.Date)
                    .ToListAsync()
            };

            return View(revenueData);
        }

        // GET: Admin/Report/BestSelling
        public async Task<IActionResult> BestSelling(DateTime? fromDate = null, DateTime? toDate = null)
        {
            // Set default date range (last 30 days)
            fromDate ??= DateTime.Today.AddDays(-30);
            toDate ??= DateTime.Today;

            var bestSellingProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.OrderDate.Date >= fromDate.Value.Date && 
                            oi.Order.OrderDate.Date <= toDate.Value.Date &&
                            oi.Order.IsPaid)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name, oi.Product.Image })
                .Select(g => new BestSellingProductItem
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    ProductImage = g.Key.Image,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.ItemPrice * oi.Quantity),
                    OrderCount = g.Count()
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(20)
                .ToListAsync();

            var viewModel = new BestSellingReportViewModel
            {
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                BestSellingProducts = bestSellingProducts
            };

            return View(viewModel);
        }

        // GET: Admin/Report/Orders
        public async Task<IActionResult> Orders(DateTime? fromDate = null, DateTime? toDate = null)
        {
            fromDate ??= DateTime.Today.AddDays(-30);
            toDate ??= DateTime.Today;

            var orderStats = await _context.Orders
                .Where(o => o.OrderDate.Date >= fromDate.Value.Date && o.OrderDate.Date <= toDate.Value.Date)
                .GroupBy(o => o.Status)
                .Select(g => new OrderStatusItem
                {
                    Status = g.Key,
                    Count = g.Count(),
                    TotalValue = g.Sum(o => o.TotalPrice)
                })
                .ToListAsync();

            var viewModel = new OrderReportViewModel
            {
                FromDate = fromDate.Value,
                ToDate = toDate.Value,
                OrderStatistics = orderStats
            };

            return View(viewModel);
        }
    }

    public class RevenueReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PaidOrders { get; set; }
        public int UnpaidOrders { get; set; }
        public List<DailyRevenueItem> DailyRevenue { get; set; } = new List<DailyRevenueItem>();
    }

    public class DailyRevenueItem
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class BestSellingReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<BestSellingProductItem> BestSellingProducts { get; set; } = new List<BestSellingProductItem>();
    }

    public class BestSellingProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ProductImage { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class OrderReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<OrderStatusItem> OrderStatistics { get; set; } = new List<OrderStatusItem>();
    }

    public class OrderStatusItem
    {
        public OrderStatus Status { get; set; }
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
    }
} 