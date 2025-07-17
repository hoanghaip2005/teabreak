// Models/Toocha/Discount.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using toocha.Models.Toocha;

namespace App.Models.Toocha
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải có mã giảm giá")]
        [StringLength(50)]
        [Display(Name = "Mã giảm giá")]
        public string Code { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        
        // Đổi tên từ DiscountValue thành Value cho rõ nghĩa hơn
        [Required(ErrorMessage = "Phải có giá trị giảm")]
        [Display(Name = "Giá trị giảm")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        [Required]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Kích hoạt")]
        public bool IsActive { get; set; }

        // ----- PHẦN CẢI TIẾN -----

        [Display(Name = "Loại khuyến mãi")]
        public DiscountType Type { get; set; }

        [Display(Name = "Sản phẩm áp dụng")]
        public int? ProductId { get; set; } // Nullable: có thể không áp dụng cho sản phẩm nào cụ thể
        
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        
        [Display(Name = "Danh mục áp dụng")]
        public int? CategoryId { get; set; } // Nullable: có thể không áp dụng cho danh mục nào cụ thể
        
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        // ----- THUỘC TÍNH BỔ SUNG -----

        [Display(Name = "Giới hạn sử dụng")]
        public int? UsageLimit { get; set; } // Null = không giới hạn

        [Display(Name = "Đã sử dụng")]
        public int UsedCount { get; set; } = 0;

        [Display(Name = "Đơn hàng tối thiểu")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinOrderAmount { get; set; } // Null = không giới hạn

        [Display(Name = "Giảm tối đa")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaxDiscountAmount { get; set; } // Cho loại phần trăm

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Người tạo")]
        public string? CreatedBy { get; set; }

        [Display(Name = "Cập nhật lần cuối")]
        public DateTime? LastUpdated { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }

        // Computed properties
        [NotMapped]
        public bool IsExpired => DateTime.Now > EndDate;

        [NotMapped]
        public bool IsNotStarted => DateTime.Now < StartDate;

        [NotMapped]
        public bool IsLimitReached => UsageLimit.HasValue && UsedCount >= UsageLimit.Value;

        [NotMapped]
        public bool IsAvailable => IsActive && !IsExpired && !IsNotStarted && !IsLimitReached;

        [NotMapped]
        public int? RemainingUsage => UsageLimit.HasValue ? Math.Max(0, UsageLimit.Value - UsedCount) : null;
    }
}