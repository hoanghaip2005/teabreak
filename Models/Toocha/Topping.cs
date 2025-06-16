using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toocha.Models.Toocha
{
    public class Topping
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal ExtraPrice { get; set; } = 0;

        [StringLength(255)]
        public string ImageUrl { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Available";

        // Navigation property
        public virtual ICollection<OrderItemTopping> OrderItemToppings { get; set; }
    }
} 