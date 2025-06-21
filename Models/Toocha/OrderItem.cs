// C:/Workspace/toocha/Models/Toocha/OrderItem.cs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using toocha.Models.Toocha;

// SỬA LỖI: Đổi namespace thành "App.Models.Toocha" để khớp với các model khác
namespace App.Models.Toocha
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18, 2)")] // Nên dùng decimal(18,2) cho nhất quán
        [Display(Name = "Giá")]
        public decimal ItemPrice { get; set; }

        [Range(0, 100, ErrorMessage = "Phần trăm đường phải từ 0 đến 100")]
        [Display(Name = "% đường")]
        public int SugarPercentage { get; set; } = 100;

        [Range(0, 100, ErrorMessage = "Phần trăm đá phải từ 0 đến 100")]
        [Display(Name = "% đá")]
        public int IcePercentage { get; set; } = 100;

        [StringLength(500)]
        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; } // Dùng string? để cho phép giá trị null

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual ICollection<OrderItemTopping> OrderItemToppings { get; set; } = new List<OrderItemTopping>();
    }
}