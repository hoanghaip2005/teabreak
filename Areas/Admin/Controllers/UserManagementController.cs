using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using App.Data;
using App.Models;

namespace App.Areas.Admin.Controllers
{
    [Authorize(Roles = RoleName.Administrator)]
    [Area("Admin")]
    [Route("/Admin/User/[action]")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public UserManagementController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Admin/User
        [Route("/Admin/User")]  // Thêm route riêng cho trang chủ
        public async Task<IActionResult> Index(int page = 1, string search = "", string role = "")
        {
            const int pageSize = 10;
            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.UserName.Contains(search) || u.Email.Contains(search) || u.PhoneNumber.Contains(search));
                ViewBag.Search = search;
            }

            var totalItems = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.UserName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Get roles for each user
            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel
                {
                    User = user,
                    Roles = userRoles.ToList()
                });
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.TotalItems = totalItems;
            ViewBag.AllRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            return View(userViewModels);
        }

        // GET: Admin/User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            var viewModel = new UserDetailsViewModel
            {
                User = user,
                Roles = roles.ToList(),
                Claims = claims.ToList()
            };

            return View(viewModel);
        }

        // POST: Admin/User/LockUnlock/5
        [HttpPost]
        public async Task<IActionResult> LockUnlock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
                {
                    // Unlock user
                    user.LockoutEnd = null;
                    StatusMessage = $"Đã mở khóa tài khoản '{user.UserName}'!";
                }
                else
                {
                    // Lock user for 1000 years
                    user.LockoutEnd = DateTime.UtcNow.AddYears(1000);
                    StatusMessage = $"Đã khóa tài khoản '{user.UserName}'!";
                }

                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/User/ManageRoles/5
        public async Task<IActionResult> ManageRoles(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();

            var viewModel = new ManageUserRolesViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRoles = userRoles.ToList(),
                AllRoles = allRoles.Select(r => r.Name).ToList()
            };

            return View(viewModel);
        }

        // POST: Admin/User/ManageRoles/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(string id, List<string> selectedRoles)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            // Remove user from all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeResult.Succeeded)
            {
                StatusMessage = "Có lỗi xảy ra khi cập nhật quyền!";
                return RedirectToAction(nameof(Index));
            }

            // Add user to selected roles
            if (selectedRoles != null && selectedRoles.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addResult.Succeeded)
                {
                    StatusMessage = "Có lỗi xảy ra khi cập nhật quyền!";
                    return RedirectToAction(nameof(Index));
                }
            }

            StatusMessage = $"Đã cập nhật quyền cho tài khoản '{user.UserName}' thành công!";
            return RedirectToAction(nameof(Index));
        }
    }

    public class UserViewModel
    {
        public AppUser User { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class UserDetailsViewModel
    {
        public AppUser User { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<System.Security.Claims.Claim> Claims { get; set; } = new List<System.Security.Claims.Claim>();
    }

    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> UserRoles { get; set; } = new List<string>();
        public List<string> AllRoles { get; set; } = new List<string>();
    }
} 