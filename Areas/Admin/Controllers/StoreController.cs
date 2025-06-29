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
    [Route("/Admin/Store/[action]")]
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Admin/Store
        [Route("/Admin/Store")]  // Thêm route riêng cho trang chủ
        public async Task<IActionResult> Index(int page = 1, string search = "")
        {
            const int pageSize = 10;
            var query = _context.Stores.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name.Contains(search) || s.Address.Contains(search) || s.City.Contains(search));
                ViewBag.Search = search;
            }

            var totalItems = await query.CountAsync();
            var stores = await query
                .OrderBy(s => s.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;

            return View(stores);
        }

        // GET: Admin/Store/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var store = await _context.Stores
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (store == null) return NotFound();

            return View(store);
        }

        // GET: Admin/Store/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Store/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,City,District,PhoneNumber,OpeningHours,IsActive,Latitude,Longitude,Region")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                
                StatusMessage = $"Đã tạo cửa hàng '{store.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // GET: Admin/Store/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var store = await _context.Stores.FindAsync(id);
            if (store == null) return NotFound();
            
            return View(store);
        }

        // POST: Admin/Store/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Address,City,District,PhoneNumber,OpeningHours,IsActive,Latitude,Longitude,Region")] Store store)
        {
            if (id != store.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                    
                    StatusMessage = $"Đã cập nhật cửa hàng '{store.Name}' thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(store);
        }

        // POST: Admin/Store/ToggleActive/5
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                store.IsActive = !store.IsActive;
                await _context.SaveChangesAsync();
                
                var status = store.IsActive ? "kích hoạt" : "tạm ngưng";
                StatusMessage = $"Đã {status} cửa hàng '{store.Name}'!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Store/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var store = await _context.Stores
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (store == null) return NotFound();

            return View(store);
        }

        // POST: Admin/Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store != null)
            {
                // Kiểm tra xem có đơn hàng nào đang sử dụng cửa hàng này không
                var ordersCount = await _context.Orders.CountAsync(o => o.StoreId == id);
                if (ordersCount > 0)
                {
                    StatusMessage = $"Không thể xóa cửa hàng '{store.Name}' vì còn {ordersCount} đơn hàng liên quan!";
                    return RedirectToAction(nameof(Index));
                }

                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();
                StatusMessage = $"Đã xóa cửa hàng '{store.Name}' thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.Id == id);
        }
    }
} 