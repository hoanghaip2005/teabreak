// Models/Toocha/Product.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using toocha.Models.Toocha;

namespace App.Models.Toocha
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Phải nhập tên sản phẩm")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "{0} phải dài từ {2} đến {1} ký tự")]
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Phải nhập giá sản phẩm")]
        [Range(0, double.MaxValue, ErrorMessage = "{0} phải lớn hơn hoặc bằng {1}")]
        [Display(Name = "Giá")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalePrice { get; set; }

        [Display(Name = "Hình ảnh")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Phải chọn danh mục")]
        [Display(Name = "Danh mục")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        // ----- PHẦN CẢI TIẾN -----

        [Display(Name = "Số lượng tồn kho")]
        public int StockQuantity { get; set; } = 0;

        [Display(Name = "Hiển thị / Kinh doanh")]
        public bool IsPublished { get; set; } = true;
    }
}