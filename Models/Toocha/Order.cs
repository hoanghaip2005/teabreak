using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace toocha.Models.Toocha
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [StringLength(100)]
        public string PaymentMethod { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ExpectedDeliveryAt { get; set; }

        public DateTime? ActualDeliveryAt { get; set; }

        // Navigation property
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
} 