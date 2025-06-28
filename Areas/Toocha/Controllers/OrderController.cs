using System.Diagnostics;
using App.Models;
using App.Models.Toocha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using toocha.Models;
using toocha.Models.Toocha;
using Microsoft.AspNetCore.Authorization;
using App.Data;
using App.Services;

namespace toocha.Areas.Toocha.Controllers;

[Area("Toocha")]
public class OrderController : Controller
{
    public async Task<IActionResult> Index()
    {
        // Lấy tất cả danh mục với sản phẩm
        var categories = await _context.Categories
            .Include(c => c.Products)
            .Where(c => c.Products.Any(p => p.IsPublished))
            .ToListAsync();

        return View(categories);
    }

    // using Microsoft.EntityFrameworkCore; // Thêm vào đầu file nếu chưa có
    // using App.Models; // Thêm vào đầu file nếu chưa có

    // ...bên trong class OrderController
    private readonly AppDbContext _context;
    private readonly ILocationService _locationService;

    public OrderController(AppDbContext context, ILocationService locationService)
    {
        _context = context;
        _locationService = locationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductDetails(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        // Lấy tất cả các size và topping đang hoạt động
        var sizes = await _context.Sizes.ToListAsync();
        var toppings = await _context.Toppings.Where(t => t.Status == "Available").ToListAsync();

        // Trả về dữ liệu dưới dạng JSON
        return Json(new {
            product = new {
                id = product.Id,
                name = product.Name,
                price = product.Price,
                description = product.Description,
                image = product.Image
            },
            sizes = sizes,
            toppings = toppings
        });
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderRequest request)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null,
                OrderDate = DateTime.Now,
                TotalPrice = request.Total,
                ShippingAddress = request.CustomerAddress,
                PhoneNumber = request.CustomerPhone,
                Status = OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                IsPaid = false
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Lưu để có OrderId

            // Tạo OrderItems
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ItemPrice = item.TotalPrice,
                    SugarPercentage = item.Sugar,
                    IcePercentage = item.Ice,
                    Notes = $"Size: {item.Options.Size}"
                };

                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync(); // Lưu để có OrderItemId

                // Tạo OrderItemToppings nếu có
                if (item.Options.Toppings != null && item.Options.Toppings.Any())
                {
                    foreach (var toppingName in item.Options.Toppings)
                    {
                        // Tìm topping ID từ tên
                        var toppingSearchName = toppingName.Split(' ')[0];
                        var topping = await _context.Toppings.FirstOrDefaultAsync(t => t.Name.Contains(toppingSearchName));
                        if (topping != null)
                        {
                            var orderItemTopping = new OrderItemTopping
                            {
                                OrderItemId = orderItem.Id,
                                ToppingId = topping.Id
                            };

                            _context.OrderItemToppings.Add(orderItemTopping);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Json(new { 
                success = true, 
                message = "Đặt hàng thành công!",
                orderId = order.Id
            });
        }
        catch (Exception ex)
        {
            return Json(new { 
                success = false, 
                message = "Có lỗi xảy ra khi đặt hàng: " + ex.Message
            });
        }
    }

    // ===== PHẦN QUẢN LÝ ĐƠN HÀNG CHO NGƯỜI BÁN =====

    [Authorize(Roles = $"{RoleName.Administrator},{RoleName.Seller},{RoleName.Manager}")]
    public async Task<IActionResult> OrderManagement(string status = "", string search = "", DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 20)
    {
        var query = _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .AsQueryable();

        // Lọc theo trạng thái
        if (!string.IsNullOrEmpty(status) && Enum.TryParse<OrderStatus>(status, out var orderStatus))
        {
            query = query.Where(o => o.Status == orderStatus);
        }

        // Tìm kiếm theo tên khách hàng, số điện thoại, địa chỉ
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(o => 
                o.PhoneNumber.Contains(search) ||
                o.ShippingAddress.Contains(search) ||
                (o.User != null && o.User.UserName.Contains(search)));
        }

        // Lọc theo ngày
        if (fromDate.HasValue)
        {
            query = query.Where(o => o.OrderDate >= fromDate.Value);
        }
        if (toDate.HasValue)
        {
            query = query.Where(o => o.OrderDate <= toDate.Value.AddDays(1));
        }

        // Sắp xếp theo ngày đặt hàng mới nhất
        query = query.OrderByDescending(o => o.OrderDate);

        // Phân trang
        var totalOrders = await query.CountAsync();
        var orders = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.CurrentStatus = status;
        ViewBag.CurrentSearch = search;
        ViewBag.FromDate = fromDate;
        ViewBag.ToDate = toDate;
        ViewBag.CurrentPage = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalOrders / pageSize);
        ViewBag.TotalOrders = totalOrders;

        return View(orders);
    }

    [Authorize(Roles = $"{RoleName.Administrator},{RoleName.Seller},{RoleName.Manager}")]
    public async Task<IActionResult> OrderDetails(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.OrderItemToppings)
                    .ThenInclude(oit => oit.Topping)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy đơn hàng";
            return RedirectToAction(nameof(OrderManagement));
        }

        return View(order);
    }

    [HttpPost]
    [Authorize(Roles = $"{RoleName.Administrator},{RoleName.Seller},{RoleName.Manager}")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return Json(new { success = false, message = "Không tìm thấy đơn hàng" });
            }

            // Validate status transition
            if (!IsValidStatusTransition(order.Status, newStatus))
            {
                return Json(new { success = false, message = "Không thể chuyển đổi trạng thái này" });
            }

            order.Status = newStatus;
            
            // Nếu trạng thái là Delivered thì đánh dấu đã thanh toán (với COD)
            if (newStatus == OrderStatus.Delivered && order.PaymentMethod == "COD")
            {
                order.IsPaid = true;
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                message = "Cập nhật trạng thái thành công",
                newStatus = newStatus.ToString()
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
        }
    }

    private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        // Định nghĩa các chuyển đổi trạng thái hợp lệ
        return currentStatus switch
        {
            OrderStatus.Pending => newStatus == OrderStatus.Processing || newStatus == OrderStatus.Cancelled,
            OrderStatus.Processing => newStatus == OrderStatus.Shipped || newStatus == OrderStatus.Cancelled,
            OrderStatus.Shipped => newStatus == OrderStatus.Delivered || newStatus == OrderStatus.Failed,
            OrderStatus.Delivered => false, // Không thể thay đổi sau khi đã giao
            OrderStatus.Cancelled => false, // Không thể thay đổi sau khi đã hủy
            OrderStatus.Failed => newStatus == OrderStatus.Processing, // Có thể xử lý lại
            _ => false
        };
    }

    [HttpGet]
    [Authorize(Roles = $"{RoleName.Administrator},{RoleName.Seller},{RoleName.Manager}")]
    public async Task<IActionResult> GetOrderStats()
    {
        var today = DateTime.Today;
        var thisMonth = new DateTime(today.Year, today.Month, 1);

        var stats = new
        {
            TodayOrders = await _context.Orders.CountAsync(o => o.OrderDate.Date == today),
            TodayRevenue = await _context.Orders
                .Where(o => o.OrderDate.Date == today && o.IsPaid)
                .SumAsync(o => o.TotalPrice),
            MonthOrders = await _context.Orders.CountAsync(o => o.OrderDate >= thisMonth),
            MonthRevenue = await _context.Orders
                .Where(o => o.OrderDate >= thisMonth && o.IsPaid)
                .SumAsync(o => o.TotalPrice),
            PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
            ProcessingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Processing)
        };

        return Json(stats);
    }

    // ===== SHIPPING & LOCATION APIs =====

    [HttpPost]
    public async Task<IActionResult> GetNearestStores([FromBody] LocationRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Address))
            {
                return Json(new { success = false, message = "Vui lòng nhập địa chỉ giao hàng" });
            }

            // Geocode địa chỉ để lấy tọa độ
            var coordinates = await _locationService.GeocodeAddress(request.Address);
            if (coordinates == null)
            {
                return Json(new { success = false, message = "Không thể xác định vị trí từ địa chỉ này" });
            }

            // Lấy các cửa hàng gần nhất (tăng limit lên 15)
            var nearestStores = await _locationService.GetNearestStores(
                coordinates.Value.latitude, 
                coordinates.Value.longitude, 
                15
            );

            if (nearestStores.Count == 0)
            {
                return Json(new { 
                    success = false, 
                    message = "Không có cửa hàng nào trong phạm vi giao hàng 35km. Vui lòng chọn địa chỉ khác hoặc liên hệ hotline để được hỗ trợ.",
                    maxDistance = 35
                });
            }

            var result = nearestStores.Select(s => new
            {
                storeId = s.Store.Id,
                name = s.Store.Name,
                address = s.Store.Address,
                phone = s.Store.PhoneNumber,
                distance = Math.Round(s.Distance, 1),
                shippingFee = s.ShippingFee,
                openingHours = s.Store.OpeningHours,
                city = s.Store.City,
                district = s.Store.District,
                isSupported = s.IsSupported
            }).ToList();

            return Json(new { 
                success = true, 
                stores = result,
                customerLocation = new {
                    latitude = coordinates.Value.latitude,
                    longitude = coordinates.Value.longitude,
                    address = request.Address
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CalculateShippingFee([FromBody] ShippingCalculationRequest request)
    {
        try
        {
            var store = await _context.Stores.FindAsync(request.StoreId);
            if (store == null)
            {
                return Json(new { success = false, message = "Không tìm thấy cửa hàng" });
            }

            var coordinates = await _locationService.GeocodeAddress(request.DeliveryAddress);
            if (coordinates == null)
            {
                return Json(new { success = false, message = "Không thể xác định vị trí giao hàng" });
            }

            var distance = _locationService.CalculateDistance(
                store.Latitude, store.Longitude,
                coordinates.Value.latitude, coordinates.Value.longitude
            );

            var shippingFee = _locationService.CalculateShippingFee(distance);

            return Json(new { 
                success = true, 
                distance = Math.Round(distance, 1),
                shippingFee = shippingFee,
                storeName = store.Name,
                storeAddress = store.Address
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra: " + ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrderWithShipping([FromBody] PlaceOrderWithShippingRequest request)
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            // Validate store
            var store = await _context.Stores.FindAsync(request.StoreId);
            if (store == null)
            {
                return Json(new { success = false, message = "Không tìm thấy cửa hàng" });
            }

            // Geocode delivery address
            var deliveryCoordinates = await _locationService.GeocodeAddress(request.CustomerAddress);
            if (deliveryCoordinates == null)
            {
                return Json(new { success = false, message = "Không thể xác định vị trí giao hàng" });
            }

            // Calculate distance and shipping fee
            var distance = _locationService.CalculateDistance(
                store.Latitude, store.Longitude,
                deliveryCoordinates.Value.latitude, deliveryCoordinates.Value.longitude
            );
            var shippingFee = _locationService.CalculateShippingFee(distance);

            // Tạo đơn hàng mới
            var order = new Order
            {
                UserId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null,
                OrderDate = DateTime.Now,
                TotalPrice = request.Subtotal + shippingFee,
                ShippingAddress = request.CustomerAddress,
                PhoneNumber = request.CustomerPhone,
                Status = OrderStatus.Pending,
                PaymentMethod = request.PaymentMethod,
                IsPaid = false,
                // New shipping fields
                StoreId = request.StoreId,
                ShippingFee = shippingFee,
                Distance = (decimal)distance,
                DeliveryLatitude = deliveryCoordinates.Value.latitude,
                DeliveryLongitude = deliveryCoordinates.Value.longitude
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Tạo OrderItems (giữ nguyên logic cũ)
            foreach (var item in request.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ItemPrice = item.TotalPrice,
                    SugarPercentage = item.Sugar,
                    IcePercentage = item.Ice,
                    Notes = $"Size: {item.Options.Size}"
                };

                _context.OrderItems.Add(orderItem);
                await _context.SaveChangesAsync();

                // Tạo OrderItemToppings nếu có
                if (item.Options.Toppings != null && item.Options.Toppings.Any())
                {
                    foreach (var toppingName in item.Options.Toppings)
                    {
                        var toppingSearchName = toppingName.Split(' ')[0];
                        var topping = await _context.Toppings.FirstOrDefaultAsync(t => t.Name.Contains(toppingSearchName));
                        if (topping != null)
                        {
                            var orderItemTopping = new OrderItemTopping
                            {
                                OrderItemId = orderItem.Id,
                                ToppingId = topping.Id
                            };

                            _context.OrderItemToppings.Add(orderItemTopping);
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Json(new { 
                success = true, 
                message = "Đặt hàng thành công!",
                orderId = order.Id,
                totalWithShipping = order.TotalPrice,
                shippingFee = shippingFee,
                distance = Math.Round(distance, 1),
                storeName = store.Name
            });
        }
        catch (Exception ex)
        {
            return Json(new { 
                success = false, 
                message = "Có lỗi xảy ra khi đặt hàng: " + ex.Message
            });
        }
    }
}

// DTO Classes for shipping
public class LocationRequest
{
    public string Address { get; set; }
}

public class ShippingCalculationRequest
{
    public int StoreId { get; set; }
    public string DeliveryAddress { get; set; }
}

public class PlaceOrderWithShippingRequest : PlaceOrderRequest
{
    public int StoreId { get; set; }
}

// DTO Classes
public class PlaceOrderRequest
{
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerAddress { get; set; }
    public string OrderNote { get; set; }
    public string PaymentMethod { get; set; }
    public List<OrderItemRequest> Items { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Shipping { get; set; }
    public decimal Total { get; set; }
}

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public int SizeId { get; set; }
    public int Sugar { get; set; }
    public int Ice { get; set; }
    public List<string> Toppings { get; set; }
    public OrderOptionsRequest Options { get; set; }
}

public class OrderOptionsRequest
{
    public string Size { get; set; }
    public string Sugar { get; set; }
    public string Ice { get; set; }
    public List<string> Toppings { get; set; }
}
