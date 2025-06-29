using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;
using App.Models.Toocha;

namespace App.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    [Area("Admin")]
    [Route("/Admin/Order/[action]")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Admin/Order
        [Route("/Admin/Order")]  // Thêm route riêng cho trang chủ
        public async Task<IActionResult> Index(int page = 1, string search = "", OrderStatus? status = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            const int pageSize = 10;
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Store)
                .Include(o => o.OrderItems)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(o => o.Id.ToString().Contains(search) 
                    || o.PhoneNumber.Contains(search) 
                    || o.ShippingAddress.Contains(search)
                    || (o.User != null && o.User.UserName.Contains(search)));
                ViewBag.Search = search;
            }

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status);
                ViewBag.Status = status;
            }

            if (fromDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date >= fromDate.Value.Date);
                ViewBag.FromDate = fromDate;
            }

            if (toDate.HasValue)
            {
                query = query.Where(o => o.OrderDate.Date <= toDate.Value.Date);
                ViewBag.ToDate = toDate;
            }

            var totalItems = await query.CountAsync();
            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;
            ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                .Select(s => new SelectListItem 
                { 
                    Value = ((int)s).ToString(), 
                    Text = GetOrderStatusText(s) 
                }).ToList();

            return View(orders);
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Store)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (order == null) return NotFound();

            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            
            ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                .Select(s => new SelectListItem 
                { 
                    Value = ((int)s).ToString(), 
                    Text = GetOrderStatusText(s),
                    Selected = s == order.Status
                }).ToList();

            ViewBag.Stores = new SelectList(await _context.Stores.ToListAsync(), "Id", "Name", order.StoreId);

            return View(order);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OrderDate,TotalPrice,ShippingAddress,PhoneNumber,Status,PaymentMethod,IsPaid,TransactionId,StoreId,ShippingFee,Distance,DeliveryLatitude,DeliveryLongitude,Notes")] Order order)
        {
            if (id != order.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    
                    StatusMessage = $"Đã cập nhật đơn hàng #{order.Id} thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.OrderStatuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>()
                .Select(s => new SelectListItem 
                { 
                    Value = ((int)s).ToString(), 
                    Text = GetOrderStatusText(s),
                    Selected = s == order.Status
                }).ToList();

            ViewBag.Stores = new SelectList(await _context.Stores.ToListAsync(), "Id", "Name", order.StoreId);
            return View(order);
        }

        // POST: Admin/Order/UpdateStatus
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
                
                StatusMessage = $"Đã cập nhật trạng thái đơn hàng #{order.Id} thành '{GetOrderStatusText(status)}'!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Order/TogglePayment
        [HttpPost]
        public async Task<IActionResult> TogglePayment(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.IsPaid = !order.IsPaid;
                await _context.SaveChangesAsync();
                
                var paymentStatus = order.IsPaid ? "đã thanh toán" : "chưa thanh toán";
                StatusMessage = $"Đã cập nhật đơn hàng #{order.Id} thành {paymentStatus}!";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Store)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (order == null) return NotFound();

            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                StatusMessage = $"Đã xóa đơn hàng #{order.Id} thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private string GetOrderStatusText(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Pending => "Chờ xử lý",
                OrderStatus.Processing => "Đang xử lý", 
                OrderStatus.Shipped => "Đang giao hàng",
                OrderStatus.Delivered => "Đã giao hàng",
                OrderStatus.Cancelled => "Đã hủy",
                OrderStatus.Failed => "Thất bại",
                _ => status.ToString()
            };
        }
    }
} 