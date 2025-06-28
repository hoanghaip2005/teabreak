using App.Models;
using App.Models.Toocha;
using Microsoft.AspNetCore.Mvc;
using toocha.Models.Toocha;

namespace toocha.Areas.Toocha.Controllers
{
    [Area("Toocha")]
    public class DataSeedController : Controller
    {
        private readonly AppDbContext _context;

        public DataSeedController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClearData()
        {
            try
            {
                // Xóa dữ liệu theo thứ tự đúng để tránh foreign key constraints
                
                // 1. Xóa OrderItemToppings trước (có FK đến OrderItems và Toppings)
                if (_context.OrderItemToppings.Any())
                {
                    _context.OrderItemToppings.RemoveRange(_context.OrderItemToppings);
                    await _context.SaveChangesAsync();
                }

                // 2. Xóa OrderItems (có FK đến Orders và Products)
                if (_context.OrderItems.Any())
                {
                    _context.OrderItems.RemoveRange(_context.OrderItems);
                    await _context.SaveChangesAsync();
                }

                // 3. Xóa Orders
                if (_context.Orders.Any())
                {
                    _context.Orders.RemoveRange(_context.Orders);
                    await _context.SaveChangesAsync();
                }

                // 4. Xóa ProductReviews (có FK đến Products)
                if (_context.ProductReviews.Any())
                {
                    _context.ProductReviews.RemoveRange(_context.ProductReviews);
                    await _context.SaveChangesAsync();
                }

                // 5. Xóa Products (có FK đến Categories)
                if (_context.Products.Any())
                {
                    _context.Products.RemoveRange(_context.Products);
                    await _context.SaveChangesAsync();
                }

                // 6. Xóa Categories
                if (_context.Categories.Any())
                {
                    _context.Categories.RemoveRange(_context.Categories);
                    await _context.SaveChangesAsync();
                }

                // 7. Xóa Toppings và Sizes (không có FK dependencies)
                if (_context.Toppings.Any())
                {
                    _context.Toppings.RemoveRange(_context.Toppings);
                    await _context.SaveChangesAsync();
                }

                if (_context.Sizes.Any())
                {
                    _context.Sizes.RemoveRange(_context.Sizes);
                    await _context.SaveChangesAsync();
                }

                // 8. Xóa Discounts
                if (_context.Discounts.Any())
                {
                    _context.Discounts.RemoveRange(_context.Discounts);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Đã xóa hết dữ liệu thành công!" });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? "Không có thông tin chi tiết";
                var fullMessage = $"Lỗi: {ex.Message}. Chi tiết: {innerException}";
                
                return Json(new { 
                    success = false, 
                    message = fullMessage,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                // Kiểm tra xem đã có dữ liệu chưa
                if (_context.Categories.Any())
                {
                    return Json(new { success = false, message = "Dữ liệu đã tồn tại" });
                }

                // Tạo categories trước và lấy Id thực tế
                var category1 = new Category { Name = "Món Nổi Bật", Description = "Các món ăn và thức uống nổi bật nhất" };
                var category2 = new Category { Name = "Instant Milk Tea", Description = "Trà sữa pha chế nhanh" };
                var category3 = new Category { Name = "Trà Sữa", Description = "Các loại trà sữa truyền thống" };
                var category4 = new Category { Name = "Fresh Fruit Tea", Description = "Trà trái cây tươi" };
                var category5 = new Category { Name = "Macchiato Cream Cheese", Description = "Macchiato và kem phô mai" };
                var category6 = new Category { Name = "Cà Phê", Description = "Các loại cà phê đặc biệt" };
                var category7 = new Category { Name = "Ice Cream", Description = "Kem các loại" };
                var category8 = new Category { Name = "Special Menu", Description = "Menu đặc biệt" };

                _context.Categories.AddRange(new[] { category1, category2, category3, category4, category5, category6, category7, category8 });
                await _context.SaveChangesAsync();

                // Bây giờ tạo products với CategoryId thực tế
                var products = new List<Product>
                {
                    // Món Nổi Bật (45 sản phẩm) - Sửa lỗi hình ảnh
                    new Product { Name = "Bánh Tráng Hỏa Diệm Sơn", Description = "Bánh tráng đặc biệt hỏa diệm sơn", Price = 15000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-1-copy.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Xanh Chanh Leo Kem Phô Mai", Description = "Trà xanh thanh mát kết hợp với chanh leo chua ngọt và kem phô mai béo ngậy", Price = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/oolong-kem-pho-mai_75e8d3f11fb3402196416da77c8ff33a_grande.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Khoai Môn Đường Hổ", Description = "Trà sữa khoai môn thơm ngon với đường hổ đặc trưng", Price = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/06/TS_TIGER_SUGAR.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Xanh Đào Chanh Leo", Description = "Trà xanh kết hợp với đào tươi và chanh leo", Price = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Dau-Tam-Pha-Le-Tuyet-2-copy.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Boba Cheese", Description = "Trà ô long với sữa, boba và kem cheese", Price = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/tra-dua.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Đào Tiên", Description = "Trà ô long với đào tiên ngọt ngào", Price = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Socola-1-copy.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Thạch Dâu Tây - Strawberry Jelly", Description = "Thạch dâu tây mát lạnh, ngọt ngào", Price = 35000, SalePrice = 48000, Image = "https://tocotocotea.com/wp-content/uploads/2025/02/FP-1.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Sữa Nhài Đào Tiên", Description = "Trà xanh nhài với sữa và đào tiên", Price = 25000, SalePrice = 34000, Image = "https://tocotocotea.com/wp-content/uploads/2025/02/900x900-Xanh-Sua-Nhai-Dao-Tien.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Dâu Tây", Description = "Trà ô long với dâu tây tươi", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2025/02/900x900-O-Long-Dau-Tay.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Oolong Dâu Tây Kem Phô Mai", Description = "Oolong dâu tây với kem phô mai béo ngậy", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2025/02/900x900-Oolong-Dau-Tay-Kem-Pho-Mai.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Oolong Sữa Hạt Dẻ Hoàng Kim", Description = "Oolong sữa với hạt dẻ hoàng kim thơm béo", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/12/Oolong-sua-hat-de-hoang-kim_Tea.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Hồng Trà Mận Quế Hoa Khổng Lồ", Description = "Hồng trà mận quế hoa size khổng lồ", Price = 22000, Image = "https://tocotocotea.com/wp-content/uploads/2024/11/z6050430969392_bf7c447cf1f9854b92d6c9400bce07b6.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Sữa Tươi Yến Mạch", Description = "Sữa tươi yến mạch bổ dưỡng", Price = 30000, SalePrice = 39000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/Sua-Tuoi-Yen-Mach.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Sữa tươi Nếp Cẩm", Description = "Sữa tươi nếp cẩm độc đáo", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/Sua-Tuoi-Nep-Cam.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Yến Mạch", Description = "Ô long yến mạch thanh mát", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/O-Long-Yen-Mach.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Yến Mạch", Description = "Trà sữa yến mạch bổ dưỡng", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/Tra-Sua-Yen-Mach.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Nếp Cẩm", Description = "Trà sữa nếp cẩm tím đặc biệt", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/Tra-Sua-Nep-Cam.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Thạch Đào Tiên - Peach Jelly", Description = "Thạch đào tiên mát lạnh", Price = 35000, SalePrice = 48000, Image = "https://tocotocotea.com/wp-content/uploads/2024/09/z5849639195288_ab6be655af86d11d4dd5f446031cb465.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Vani Trà Sữa Trân Châu Hoàng Kim", Description = "Kem vani trà sữa với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Kem-Vani-Tra-Sua-Tran-Chau-Hoang-Kim.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Trà Sữa Trân Châu Hoàng Kim", Description = "Kem trà sữa với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Kem-Tra-Sua-Tran-Chau-Hoang-Kim.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Đào Lê Tây Bắc Khổng Lồ - 1000ml", Description = "Ô long đào lê Tây Bắc size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/O-long-dao-le-tay-bac-khong-lo.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Lê Tây Bắc Khổng Lồ - 1000ml", Description = "Xanh nhài lê Tây Bắc size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Xanh-Nhai-Le-Tay-Bac-Khong-Lo.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Đào Lê Tây Bắc", Description = "Ô long với đào lê Tây Bắc thơm ngọt", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/O-Long-Dao-Le-Tay-Bac.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Lê Tây Bắc", Description = "Trà xanh nhài với lê Tây Bắc", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Xanh-Nhai-Le-Tay-Bac.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Boba Cheese", Description = "Trà sữa với boba và kem cheese", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Tra-Sua-BoBa-Cheese.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh nhài mãng cầu", Description = "Trà xanh nhài với mãng cầu", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Xanh-Nhai-Mang-Cau.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Phê Trà Chanh Vàng", Description = "Phê trà chanh vàng tươi mát", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Phe-Tra-Chanh-Vang.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Xoài Băng Tuyết", Description = "Trà xanh nhài xoài băng tuyết", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Xanh-Nhai-Xoai-Bang-Tuyet.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Mãng Cầu Xoài", Description = "Trà xanh nhài mãng cầu xoài", Price = 30000, SalePrice = 36000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Xanh-Nhai-Mang-Cau-Xoai.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Sữa Tươi Toco", Description = "Trà xanh nhài sữa tươi Toco", Price = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Xanh-Nhai-Sua-Toco_Tea.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Tươi", Description = "Ô long sữa tươi béo ngậy", Price = 30000, SalePrice = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/O-long-sua-tuoi.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Đào Quế Hoa Kem Cheese", Description = "Ô long đào quế hoa với kem cheese", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Oolong-Dao-Que-Hoa-Kem-Cheese.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Phê Sữa Kem Cheese", Description = "Phê sữa với kem cheese béo ngậy", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/phe-sua-kem-cheese.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Kem Café Trân Châu Sợi", Description = "Ô long sữa kem café với trân châu sợi", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/O-long-sua-kem-cafe-tran-chau-soi.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Xanh Nhài Đào Tiên", Description = "Trà xanh nhài với đào tiên thơm ngon", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Tra-Xanh-Dao-Que-Hoa.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Đào Tiên Quế Hoa", Description = "Trà đào tiên với quế hoa thơm lừng", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Tra-Dao-Tien-Que-Hoa-1.png", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Tuyết Lê Khổng Lồ", Description = "Ô long tuyết lê size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2024/01/z5200313014176_88115b1d87eae335d18ada8210389e0d.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Chanh Mật Ong Giã Tay Khổng Lồ", Description = "Trà chanh mật ong giã tay size khổng lồ", Price = 35000, SalePrice = 43000, Image = "https://tocotocotea.com/wp-content/uploads/2023/12/z4967287161207_d582f10a876bcc7359666df4f2ec76c5.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Chanh Mật Ong Giã Tay", Description = "Trà chanh mật ong giã tay", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2023/11/z4925614520640_1babe5daea83472f4784ceb3cd206979.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Trân Châu Ngũ Cốc", Description = "Ô long sữa với trân châu ngũ cốc", Price = 25000, SalePrice = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2023/11/z4925614515113_5bdf67d7e4b3ee98215ea11da9b303e9.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Sữa Tươi Trân Châu Đường Hổ Khổng Lồ", Description = "Sữa tươi trân châu đường hổ size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2023/10/z4853951474939_d2f4544ce4706979c9779d466a8efc5e.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Người Bạn Xanh Sữa Nhài Khổng Lồ", Description = "Trà xanh sữa nhài size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2023/09/z4704498631032_4dc66b5cea996f0c14840a46043f161c.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Ly Vani Dâu", Description = "Kem ly vani với dâu tươi", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2023/08/z4617754537982_689908f83d0785da485d70d0307e5130.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Cafe Kem Trân Châu Hoàng Kim", Description = "Cafe kem với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/CF-Kem-Tran-Chau-HK.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Trân Châu Hoàng Kim", Description = "Kem với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/kem-tran-chau-hoang-kim.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Mận Kem Phô Mai", Description = "Ô long mận với kem phô mai", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/O-Long-Man-Kem-Pho-Mai.jpg", CategoryId = category1.Id, StockQuantity = 100, IsPublished = true },

                    // Instant Milk Tea (6 sản phẩm) - Sử dụng hình ảnh có sẵn
                    new Product { Name = "Instant Milk Tea Classic", Description = "Trà sữa instant kiểu truyền thống", Price = 20000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-1-copy.jpg", CategoryId = category2.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Instant Taro Milk Tea", Description = "Trà sữa khoai môn instant", Price = 22000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Socola-1-copy.jpg", CategoryId = category2.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Instant Chocolate Milk Tea", Description = "Trà sữa chocolate instant", Price = 23000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Dau-Tay-2-copy.jpg", CategoryId = category2.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Instant Matcha Milk Tea", Description = "Trà sữa matcha instant", Price = 24000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Xanh-Sua-Vi-Nhai-1-copy-1.jpg", CategoryId = category2.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Instant Strawberry Milk Tea", Description = "Trà sữa dâu instant", Price = 21000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-O-Long-Sua-2-copy.jpg", CategoryId = category2.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Instant Thai Milk Tea", Description = "Trà sữa Thái instant", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2021/06/TS_TIGER_SUGAR.jpg", CategoryId = category2.Id, StockQuantity = 100, IsPublished = true },

                    // Trà Sữa (19 sản phẩm) - Trích xuất từ dữ liệu static  
                    new Product { Name = "Ô Long Đào Lê Tây Bắc", Description = "Ô long với đào lê Tây Bắc thơm ngọt", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/O-Long-Dao-Le-Tay-Bac.png", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Lê Tây Bắc", Description = "Trà xanh nhài với lê Tây Bắc", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Xanh-Nhai-Le-Tay-Bac.png", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Boba Cheese", Description = "Trà sữa với boba và kem cheese", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Tra-Sua-BoBa-Cheese.png", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Trân Châu Ngũ Cốc", Description = "Ô long sữa với trân châu ngũ cốc", Price = 25000, SalePrice = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2023/11/z4925614515113_5bdf67d7e4b3ee98215ea11da9b303e9.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Sữa Tươi Trân Châu Đường Hổ Khổng Lồ", Description = "Sữa tươi trân châu đường hổ size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2023/10/z4853951474939_d2f4544ce4706979c9779d466a8efc5e.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Người Bạn Xanh Sữa Nhài Khổng Lồ", Description = "Trà xanh sữa nhài size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2023/09/z4704498631032_4dc66b5cea996f0c14840a46043f161c.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Trân Châu Đường Hổ", Description = "Trà sữa trân châu với đường hổ đặc biệt", Price = 25000, SalePrice = 33000, Image = "https://tocotocotea.com/wp-content/uploads/2023/04/tra-sua-tran-chau-duong-den.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Phô Mai Tươi", Description = "Trà sữa phô mai tươi béo ngậy", Price = 25000, SalePrice = 40000, Image = "https://tocotocotea.com/wp-content/uploads/2023/01/Tra-sua-pho-mai-tuoi.png", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Tiger Sugar", Description = "Trà sữa tiger sugar đặc trưng", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2021/06/TS_TIGER_SUGAR.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Trân Châu Hoàng Gia", Description = "Trà sữa trân châu hoàng gia cao cấp", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Tran-Chau-Hoang-Gia-1-copy.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Ba Anh Em", Description = "Trà sữa ba anh em đặc biệt", Price = 25000, SalePrice = 39000, Image = "https://tocotocotea.com/wp-content/uploads/2025/04/3-anh-em.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Trân Châu Sợi", Description = "Trà sữa với trân châu sợi", Price = 25000, SalePrice = 39000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/tra-sua-tran-chau-soi.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Kim Cương Đen Okinawa", Description = "Trà sữa kim cương đen Okinawa", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Okinawa_Moi.png", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Xanh Sữa Vị Nhài", Description = "Trà xanh sữa vị nhài thơm ngon", Price = 25000, SalePrice = 33000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Xanh-Sua-Vi-Nhai-1-copy-1.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Ô Long", Description = "Trà sữa ô long truyền thống", Price = 25000, SalePrice = 34000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-O-Long-Sua-2-copy.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Sữa Socola", Description = "Trà sữa socola ngọt ngào", Price = 25000, SalePrice = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Socola-1-copy.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà sữa dâu tây", Description = "Trà sữa dâu tây tươi ngon", Price = 25000, SalePrice = 34000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-Dau-Tay-2-copy.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà sữa", Description = "Trà sữa classic truyền thống", Price = 25000, SalePrice = 33000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Sua-1-copy.jpg", CategoryId = category3.Id, StockQuantity = 100, IsPublished = true },

                    // Fresh Fruit Tea (8 sản phẩm) - Trích xuất từ dữ liệu static
                    new Product { Name = "Trà Xanh Nhài Đào Tiên", Description = "Trà xanh nhài với đào tiên thơm ngon", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Tra-Xanh-Dao-Que-Hoa.png", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Đào Tiên Quế Hoa", Description = "Trà đào tiên với quế hoa thơm lừng", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Tra-Dao-Tien-Que-Hoa-1.png", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Tuyết Lê Khổng Lồ", Description = "Ô long tuyết lê size khổng lồ", Price = 35000, SalePrice = 45000, Image = "https://tocotocotea.com/wp-content/uploads/2024/01/z5200313014176_88115b1d87eae335d18ada8210389e0d.jpg", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Chanh Mật Ong Giã Tay Khổng Lồ", Description = "Trà chanh mật ong giã tay size khổng lồ", Price = 35000, SalePrice = 43000, Image = "https://tocotocotea.com/wp-content/uploads/2023/12/z4967287161207_d582f10a876bcc7359666df4f2ec76c5.jpg", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Chanh Mật Ong Giã Tay", Description = "Trà chanh mật ong giã tay", Price = 25000, SalePrice = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2023/11/z4925614520640_1babe5daea83472f4784ceb3cd206979.jpg", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Mận Mộc Châu Thạch Quế Hoa", Description = "Ô long mận mộc châu với thạch quế hoa", Price = 25000, SalePrice = 36000, Image = "https://tocotocotea.com/wp-content/uploads/2022/10/tra-man.jpg", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà dâu tằm pha lê tuyết", Description = "Trà dâu tằm pha lê tuyết mát lạnh", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Dau-Tam-Pha-Le-Tuyet-2-copy.jpg", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Dứa Thạch Konjac", Description = "Trà dứa với thạch konjac", Price = 25000, SalePrice = 40000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/tra-dua.jpg", CategoryId = category4.Id, StockQuantity = 100, IsPublished = true },

                    // Macchiato Cream Cheese (8 sản phẩm) - Trích xuất từ dữ liệu static
                    new Product { Name = "Ô Long Đào Quế Hoa Kem Cheese", Description = "Ô long đào quế hoa với kem cheese", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Oolong-Dao-Que-Hoa-Kem-Cheese.png", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Phê Sữa Kem Cheese", Description = "Phê sữa với kem cheese béo ngậy", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/phe-sua-kem-cheese.png", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Kem Café Trân Châu Sợi", Description = "Ô long sữa kem café với trân châu sợi", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/O-long-sua-kem-cafe-tran-chau-soi.png", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Mận Kem Phô Mai", Description = "Ô long mận với kem phô mai", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/O-Long-Man-Kem-Pho-Mai.jpg", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Kem Phô Mai", Description = "Ô long với kem phô mai classic", Price = 25000, SalePrice = 40000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/oolong-kem-pho-mai_75e8d3f11fb3402196416da77c8ff33a_grande.png", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Dâu Tằm Kem Phô Mai", Description = "Dâu tằm với kem phô mai", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/dau-tam-kem-pho-mai.png", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Hồng Trà Kem Phô Mai", Description = "Hồng trà với kem phô mai", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Hong-Tra-Kem-Pho-Mai-2-copy.jpg", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Trà Xanh Kem Phô Mai", Description = "Trà xanh với kem phô mai", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/Tra-Xanh-Kem-Pho-Mai-2-copy.jpg", CategoryId = category5.Id, StockQuantity = 100, IsPublished = true },

                    // Cà Phê (8 sản phẩm) - Sử dụng hình ảnh có sẵn
                    new Product { Name = "Jelly Milk Coffee", Description = "Cà phê sữa với thạch thơm ngon", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/11/Jelly-coffee.jpg", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Cheese Milk Coffee", Description = "Cà phê sữa với kem cheese", Price = 25000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/11/cheese-milk-coffee.png", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Cappuccino", Description = "Cappuccino truyền thống Ý", Price = 28000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/CF-Kem-Tran-Chau-HK.jpg", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Latte", Description = "Latte mềm mượt", Price = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2021/11/Jelly-coffee.jpg", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Americano", Description = "Americano đậm đà", Price = 22000, Image = "https://tocotocotea.com/wp-content/uploads/2021/11/cheese-milk-coffee.png", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Mocha", Description = "Mocha chocolate ngọt ngào", Price = 32000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/CF-Kem-Tran-Chau-HK.jpg", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Espresso", Description = "Espresso đậm đà truyền thống", Price = 20000, Image = "https://tocotocotea.com/wp-content/uploads/2021/11/Jelly-coffee.jpg", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Macchiato Coffee", Description = "Macchiato cà phê đặc biệt", Price = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2021/11/cheese-milk-coffee.png", CategoryId = category6.Id, StockQuantity = 100, IsPublished = true },

                    // Ice Cream (5 sản phẩm) - Trích xuất từ dữ liệu static
                    new Product { Name = "Kem Vani Trà Sữa Trân Châu Hoàng Kim", Description = "Kem vani trà sữa với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Kem-Vani-Tra-Sua-Tran-Chau-Hoang-Kim.png", CategoryId = category7.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Trà Sữa Trân Châu Hoàng Kim", Description = "Kem trà sữa với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2024/08/Kem-Tra-Sua-Tran-Chau-Hoang-Kim.png", CategoryId = category7.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Ly Vani Dâu", Description = "Kem ly vani với dâu tươi", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2023/08/z4617754537982_689908f83d0785da485d70d0307e5130.jpg", CategoryId = category7.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Cafe Kem Trân Châu Hoàng Kim", Description = "Cafe kem với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/CF-Kem-Tran-Chau-HK.jpg", CategoryId = category7.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Kem Trân Châu Hoàng Kim", Description = "Kem với trân châu hoàng kim", Price = 25000, Image = "https://tocotocotea.com/wp-content/uploads/2023/05/kem-tran-chau-hoang-kim.jpg", CategoryId = category7.Id, StockQuantity = 100, IsPublished = true },

                    // Special Menu (11 sản phẩm) - Trích xuất từ dữ liệu static
                    new Product { Name = "Oolong Dâu Tây Kem Phô Mai", Description = "Oolong dâu tây với kem phô mai béo ngậy", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2025/02/900x900-Oolong-Dau-Tay-Kem-Pho-Mai.jpg", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Oolong Sữa Hạt Dẻ Hoàng Kim", Description = "Oolong sữa với hạt dẻ hoàng kim thơm béo", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/12/Oolong-sua-hat-de-hoang-kim_Tea.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Sữa Tươi Yến Mạch", Description = "Sữa tươi yến mạch bổ dưỡng", Price = 30000, SalePrice = 39000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/Sua-Tuoi-Yen-Mach.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Sữa tươi Nếp Cẩm", Description = "Sữa tươi nếp cẩm độc đáo", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/10/Sua-Tuoi-Nep-Cam.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Mãng Cầu Xoài", Description = "Trà xanh nhài mãng cầu xoài", Price = 30000, SalePrice = 36000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Xanh-Nhai-Mang-Cau-Xoai.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Xanh Nhài Sữa Tươi Toco", Description = "Trà xanh nhài sữa tươi Toco", Price = 30000, Image = "https://tocotocotea.com/wp-content/uploads/2024/06/Xanh-Nhai-Sua-Toco_Tea.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Tươi", Description = "Ô long sữa tươi béo ngậy", Price = 30000, SalePrice = 35000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/O-long-sua-tuoi.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Đào Quế Hoa Kem Cheese", Description = "Ô long đào quế hoa với kem cheese", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/Oolong-Dao-Que-Hoa-Kem-Cheese.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Phê Sữa Kem Cheese", Description = "Phê sữa với kem cheese béo ngậy", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/phe-sua-kem-cheese.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Ô Long Sữa Kem Café Trân Châu Sợi", Description = "Ô long sữa kem café với trân châu sợi", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2024/04/O-long-sua-kem-cafe-tran-chau-soi.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true },
                    new Product { Name = "Dâu Tằm Kem Phô Mai", Description = "Dâu tằm với kem phô mai", Price = 30000, SalePrice = 38000, Image = "https://tocotocotea.com/wp-content/uploads/2021/01/dau-tam-kem-pho-mai.png", CategoryId = category8.Id, StockQuantity = 100, IsPublished = true }
                };

                _context.Products.AddRange(products);
                await _context.SaveChangesAsync();

                // Tạo sizes
                var sizes = new List<Size>
                {
                    new Size { Name = "Size M", ExtraPrice = 0 },
                    new Size { Name = "Size L", ExtraPrice = 5000 },
                    new Size { Name = "Size XL", ExtraPrice = 10000 }
                };

                _context.Sizes.AddRange(sizes);
                await _context.SaveChangesAsync();

                // Tạo toppings với ImageUrl
                var toppings = new List<Topping>
                {
                    new Topping { Name = "Trân châu đen", ExtraPrice = 5000, ImageUrl = "https://images.unsplash.com/photo-1569949381669-ecf31ae8e613?w=200", Status = "Available" },
                    new Topping { Name = "Trân châu trắng", ExtraPrice = 5000, ImageUrl = "https://images.unsplash.com/photo-1589447770645-a88816652889?w=200", Status = "Available" },
                    new Topping { Name = "Thạch dừa", ExtraPrice = 7000, ImageUrl = "https://images.unsplash.com/photo-1597659840241-37e2b9c2f55f?w=200", Status = "Available" },
                    new Topping { Name = "Thạch konjac", ExtraPrice = 7000, ImageUrl = "https://images.unsplash.com/photo-1576618148400-f54bed99fcfd?w=200", Status = "Available" },
                    new Topping { Name = "Pudding", ExtraPrice = 8000, ImageUrl = "https://images.unsplash.com/photo-1551024506-0bccd828d307?w=200", Status = "Available" },
                    new Topping { Name = "Kem cheese", ExtraPrice = 10000, ImageUrl = "https://images.unsplash.com/photo-1559181567-c3190ca9959b?w=200", Status = "Available" },
                    new Topping { Name = "Sương sáo", ExtraPrice = 6000, ImageUrl = "https://images.unsplash.com/photo-1582716401301-b2407dc7563d?w=200", Status = "Available" }
                };

                _context.Toppings.AddRange(toppings);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Đã seed dữ liệu thành công!" });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException?.Message ?? "Không có thông tin chi tiết";
                var fullMessage = $"Lỗi khi seed dữ liệu: {ex.Message}. Chi tiết: {innerException}";
                
                return Json(new { 
                    success = false, 
                    message = fullMessage,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
} 