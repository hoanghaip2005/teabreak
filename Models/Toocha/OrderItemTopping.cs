using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Models.Toocha;

namespace toocha.Models.Toocha
{
    public class OrderItemTopping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        [Required]
        public int ToppingId { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ToppingPrice { get; set; }

        // Navigation properties
        [ForeignKey("OrderItemId")]
        public virtual OrderItem OrderItem { get; set; }

        [ForeignKey("ToppingId")]
        public virtual Topping Topping { get; set; }
    }
} 