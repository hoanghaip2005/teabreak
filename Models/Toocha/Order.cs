// Models/Toocha/Order.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using toocha.Models.Toocha;

namespace App.Models.Toocha
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser? User { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Tổng tiền")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Phải nhập địa chỉ giao hàng")]
        [StringLength(255)]
        [Display(Name = "Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "Phải nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        // ----- PHẦN CẢI TIẾN -----

        [Display(Name = "Trạng thái đơn hàng")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [StringLength(50)]
        [Display(Name = "Phương thức thanh toán")]
        public string? PaymentMethod { get; set; } // Ví dụ: "COD", "Credit Card", "MoMo"

        [Display(Name = "Đã thanh toán")]
        public bool IsPaid { get; set; } = false;

        [StringLength(255)]
        [Display(Name = "Mã giao dịch")]
        public string? TransactionId { get; set; }

        // Store information
        [Display(Name = "Chi nhánh")]
        public int? StoreId { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store? Store { get; set; }

        [Display(Name = "Phí giao hàng")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ShippingFee { get; set; } = 0;

        [Display(Name = "Khoảng cách (km)")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal? Distance { get; set; }

        [Display(Name = "Vĩ độ giao hàng")]
        public double? DeliveryLatitude { get; set; }

        [Display(Name = "Kinh độ giao hàng")]
        public double? DeliveryLongitude { get; set; }

        [StringLength(1000)]
        [Display(Name = "Ghi chú đơn hàng")]
        public string? Notes { get; set; }
    }
}