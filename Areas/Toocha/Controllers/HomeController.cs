using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Toocha;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Get active stores for map display
            var stores = await _context.Stores
                .Where(s => s.IsActive)
                .OrderBy(s => s.Region)
                .ThenBy(s => s.Name)
                .ToListAsync();

            ViewBag.Stores = stores;
            ViewBag.StoresCount = stores.Count;

            // Get featured products for best seller section
            var featuredProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsPublished && p.StockQuantity > 0)
                .OrderBy(p => Guid.NewGuid()) // Random order
                .Take(8)
                .ToListAsync();

            ViewBag.FeaturedProducts = featuredProducts;
            
            return View();
        }

        // Debug API to check stores data
        [HttpGet]
        public async Task<IActionResult> GetStoresData()
        {
            var allStores = await _context.Stores.ToListAsync();
            var activeStores = await _context.Stores.Where(s => s.IsActive).ToListAsync();
            
            return Json(new {
                TotalStores = allStores.Count,
                ActiveStores = activeStores.Count,
                Stores = activeStores.Select(s => new {
                    s.Id,
                    s.Name,
                    s.Address,
                    s.Latitude,
                    s.Longitude,
                    s.Region,
                    s.IsActive
                }).ToList()
            });
        }
    }
}