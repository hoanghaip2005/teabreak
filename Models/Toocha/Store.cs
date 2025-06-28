using System.ComponentModel.DataAnnotations;

namespace App.Models.Toocha
{
    public class Store
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tên cửa hàng")]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Vĩ độ")]
        public double Latitude { get; set; }

        [Required]
        [Display(Name = "Kinh độ")]
        public double Longitude { get; set; }

        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Giờ mở cửa")]
        public string? OpeningHours { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; } = true;

        [StringLength(10)]
        [Display(Name = "Miền")]
        public string? Region { get; set; } // MB: Miền Bắc, MN: Miền Nam

        [Display(Name = "Thành phố")]
        public string? City { get; set; }

        [Display(Name = "Quận/Huyện")]
        public string? District { get; set; }

        [Display(Name = "Phường/Xã")]
        public string? Ward { get; set; }

        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
} 