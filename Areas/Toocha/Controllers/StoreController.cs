using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Models.Toocha;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ToochaTea()
        {
            var stores = await _context.Stores
                .Where(s => s.IsActive)
                .OrderBy(s => s.Region)
                .ThenBy(s => s.Name)
                .ToListAsync();

            return View(stores);
        }

        [Route("toocha/store/icecream")]
        [Route("toocha/store/toochaicream")]
        public async Task<IActionResult> ToochaIcream()
        {
            // Hiện tại sử dụng dữ liệu tĩnh từ view
            // Sau này có thể mở rộng để lấy dữ liệu từ database
            var stores = await _context.Stores
                .Where(s => s.IsActive)
                .OrderBy(s => s.Region)
                .ThenBy(s => s.Name)
                .ToListAsync();

            ViewData["Title"] = "Cửa Hàng TocoToco Ice Cream & Coffee";
            return View(stores);
        }

        // API endpoint để lấy danh sách stores theo filter
        [HttpGet]
        public async Task<IActionResult> GetStores(string region = "ALL", string search = "")
        {
            var query = _context.Stores.Where(s => s.IsActive);

            // Filter theo region
            if (!string.IsNullOrEmpty(region) && region != "ALL")
            {
                query = query.Where(s => s.Region == region);
            }

            // Search theo tên hoặc địa chỉ
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(s => 
                    s.Name.ToLower().Contains(search) || 
                    s.Address.ToLower().Contains(search));
            }

            var stores = await query
                .OrderBy(s => s.Region)
                .ThenBy(s => s.Name)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Address,
                    s.Latitude,
                    s.Longitude,
                    s.PhoneNumber,
                    s.OpeningHours,
                    s.Region,
                    s.City,
                    s.District,
                    s.Ward
                })
                .ToListAsync();

            return Json(stores);
        }

        // API endpoint để lấy thông tin chi tiết một store
        [HttpGet]
        public async Task<IActionResult> GetStoreDetail(int id)
        {
            var store = await _context.Stores
                .Where(s => s.Id == id && s.IsActive)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Address,
                    s.Latitude,
                    s.Longitude,
                    s.PhoneNumber,
                    s.OpeningHours,
                    s.Region,
                    s.City,
                    s.District,
                    s.Ward,
                    s.Notes
                })
                .FirstOrDefaultAsync();

            if (store == null)
            {
                return NotFound();
            }

            return Json(store);
        }
    }
}