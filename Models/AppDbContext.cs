using App.Models;
using App.Models.Toocha;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using toocha.Models.Toocha;

namespace App.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.Id); // Ánh xạ cột UserId
            });

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>()
                .HasMaxLength(50); // Giới hạn độ dài của chuỗi

            modelBuilder.Entity<Discount>()
                .Property(d => d.Type)
                .HasConversion<string>()
                .HasMaxLength(50); // Giới hạn độ dài của chuỗi

            // 2. Thêm chỉ mục (Index) để tối ưu tốc độ truy vấn
            modelBuilder.Entity<Product>(entity =>
            {
                // Tăng tốc tìm kiếm sản phẩm theo tên
                entity.HasIndex(p => p.Name).IsUnique(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                // Tăng tốc lọc đơn hàng theo ngày
                entity.HasIndex(o => o.OrderDate);
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                // Mã giảm giá phải là duy nhất và được index để tìm nhanh
                entity.HasIndex(d => d.Code).IsUnique();
            });
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemTopping> OrderItemToppings { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }

    }
}