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
    [Route("/Admin/Product/[action]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Admin/Product
        [Route("/Admin/Product")]  // Thêm route riêng cho trang chủ
        public async Task<IActionResult> Index(int page = 1, string search = "", int? categoryId = null)
        {
            const int pageSize = 10;
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
                ViewBag.Search = search;
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(p => p.CategoryId == categoryId);
                ViewBag.CategoryId = categoryId;
            }

            var totalItems = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");

            return View(products);
        }

        // GET: Admin/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Admin/Product/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,SalePrice,CategoryId,StockQuantity,IsPublished")] Product product, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Xử lý upload hình ảnh
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img", "products");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                    
                    product.Image = "/img/products/" + uniqueFileName;
                }
                else
                {
                    product.Image = "/img/no-image.png"; // Hình mặc định
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                
                StatusMessage = $"Đã tạo sản phẩm '{product.Name}' thành công!";
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,SalePrice,Image,CategoryId,StockQuantity,IsPublished")] Product product, IFormFile? imageFile)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Xử lý upload hình ảnh mới
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img", "products");
                        Directory.CreateDirectory(uploadsFolder);
                        
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                        
                        // Xóa hình cũ nếu có
                        if (!string.IsNullOrEmpty(product.Image) && product.Image != "/img/no-image.png")
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        
                        product.Image = "/img/products/" + uniqueFileName;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    
                    StatusMessage = $"Đã cập nhật sản phẩm '{product.Name}' thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                // Xóa hình ảnh nếu có
                if (!string.IsNullOrEmpty(product.Image) && product.Image != "/img/no-image.png")
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                StatusMessage = $"Đã xóa sản phẩm '{product.Name}' thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Product/TogglePublish/5
        [HttpPost]
        public async Task<IActionResult> TogglePublish(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsPublished = !product.IsPublished;
                await _context.SaveChangesAsync();
                
                var status = product.IsPublished ? "hiển thị" : "ẩn";
                StatusMessage = $"Đã {status} sản phẩm '{product.Name}'!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
} 