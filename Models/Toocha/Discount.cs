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
    }
}