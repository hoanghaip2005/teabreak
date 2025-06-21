// Models/Toocha/OrderStatus.cs

namespace App.Models.Toocha
{
    public enum OrderStatus
    {
        Pending,        // Mới tạo, chờ xác nhận
        Processing,     // Đang xử lý
        Shipped,        // Đã giao cho đơn vị vận chuyển
        Delivered,      // Đã giao thành công
        Cancelled,      // Đã hủy
        Failed          // Thất bại
    }
}