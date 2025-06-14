using App.Data;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

string? connectString = builder.Configuration.GetConnectionString("AppMvcCollectionString");

MyGlobalConfig.ContentRootPath = builder.Environment.ContentRootPath;



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectString);
});


builder.Services.AddIdentity<AppUser, IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

// Configure email sender service
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailSender, SendMailService>();

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = false;            // Tắt xác thực email
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = false;         // Tắt xác thực tài khoản
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login/";
    options.LogoutPath = "/Account/LogOff";
    options.AccessDeniedPath = "/khongduoctruycap.html";
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.SlidingExpiration = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() 
        ? CookieSecurePolicy.None 
        : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthentication()
        .AddGoogle(options =>
        {
            var gconfig = builder.Configuration.GetSection("Authentication:Google");
            options.ClientId = gconfig["ClientId"];
            options.ClientSecret = gconfig["ClientSecret"];
            options.CallbackPath = "/dang-nhap-tu-google";
        })
        .AddFacebook(options =>
        {
            var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
            options.AppId = fconfig["AppId"];
            options.AppSecret = fconfig["AppSecret"];
            options.CallbackPath = "/dang-nhap-tu-facebook";
        });

builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
builder.Services.AddAuthorization(options => {
    options.AddPolicy("ViewManageMenu", builder => {
        builder.RequireAuthenticatedUser();
        builder.RequireRole(RoleName.Administrator);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

// Add static files from Areas directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "Areas")),
    RequestPath = "/Areas"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Toocha}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
public static class MyGlobalConfig
{
    public static string ContentRootPath { get; set; } = string.Empty;
} 