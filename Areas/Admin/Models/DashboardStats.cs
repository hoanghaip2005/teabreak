using System.ComponentModel.DataAnnotations;
using App.Models.Toocha;

namespace App.Areas.Admin.Models
{
    public class DashboardStats
    {
        [Display(Name = "Tổng sản phẩm")]
        public int TotalProducts { get; set; }

        [Display(Name = "Tổng đơn hàng")]
        public int TotalOrders { get; set; }

        [Display(Name = "Tổng danh mục")]
        public int TotalCategories { get; set; }

        [Display(Name = "Tổng cửa hàng")]
        public int TotalStores { get; set; }

        [Display(Name = "Tổng người dùng")]
        public int TotalUsers { get; set; }

        [Display(Name = "Đơn hàng hôm nay")]
        public int TodayOrders { get; set; }

        [Display(Name = "Đơn chờ xử lý")]
        public int PendingOrders { get; set; }

        [Display(Name = "Tổng doanh thu")]
        public decimal TotalRevenue { get; set; }

        [Display(Name = "Doanh thu hôm nay")]
        public decimal TodayRevenue { get; set; }

        [Display(Name = "Đơn hàng hoàn thành")]
        public int CompletedOrders { get; set; }

        [Display(Name = "Tỷ lệ hoàn thành")]
        public double CompletionRate => TotalOrders > 0 ? (double)CompletedOrders / TotalOrders * 100 : 0;

        // List of active stores for map display
        public List<Store> Stores { get; set; } = new List<Store>();
    }
} 