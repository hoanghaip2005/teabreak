# Toocha Admin System

## Giới thiệu

Hệ thống quản trị dành cho Admin và nhân viên của Toocha Tea & Ice Cream, cung cấp giao diện quản lý hoàn chỉnh với navigation phân cấp có đường line nối.

## Tính năng chính

### 📊 Dashboard
- Thống kê tổng quan về sản phẩm, đơn hàng, khách hàng
- Biểu đồ doanh thu 7 ngày qua
- Biểu đồ trạng thái đơn hàng
- Danh sách đơn hàng gần đây
- Thao tác nhanh

### 🏷️ Quản lý Danh mục
- Thêm, sửa, xóa danh mục sản phẩm
- Tìm kiếm và phân trang
- Hiển thị số sản phẩm trong mỗi danh mục

### 📦 Quản lý Sản phẩm
- CRUD đầy đủ cho sản phẩm
- Upload và quản lý hình ảnh
- Quản lý giá bán và giá khuyến mãi
- Quản lý tồn kho
- Ẩn/hiện sản phẩm
- Lọc theo danh mục
- Tìm kiếm nâng cao

### 🛒 Quản lý Đơn hàng
- Xem tất cả đơn hàng
- Lọc theo trạng thái và ngày
- Cập nhật trạng thái đơn hàng
- Quản lý thanh toán
- Xem chi tiết đơn hàng

### 🏪 Quản lý Cửa hàng
- Thêm, sửa thông tin cửa hàng
- Quản lý trạng thái hoạt động
- Thông tin địa chỉ và liên hệ

### 👥 Quản lý Người dùng
- Quản lý tài khoản người dùng
- Phân quyền vai trò
- Khóa/mở khóa tài khoản
- Xem chi tiết thông tin

## Thiết kế Navigation

### 🎨 Navigation Bar trái với thiết kế phân cấp
- **Đường line thẳng**: Kết nối từ menu cha đến menu con
- **Connector dots**: Điểm nối tại mỗi menu con
- **Hover effects**: Hiệu ứng khi di chuột
- **Active states**: Highlight menu đang được chọn
- **Responsive**: Tự động ẩn/hiện trên mobile

### 📱 Responsive Design
- Mobile-first approach
- Sidebar thu gọn trên thiết bị nhỏ
- Touch-friendly interface
- Optimized cho tablet và desktop

## Cấu trúc thư mục

```
Areas/Admin/
├── Controllers/
│   ├── HomeController.cs           # Dashboard
│   ├── CategoryController.cs       # Quản lý danh mục
│   ├── ProductController.cs        # Quản lý sản phẩm
│   ├── OrderController.cs          # Quản lý đơn hàng
│   ├── StoreController.cs          # Quản lý cửa hàng
│   └── UserManagementController.cs # Quản lý người dùng
├── Views/
│   ├── _ViewImports.cshtml
│   ├── _ViewStart.cshtml
│   ├── Shared/
│   │   └── _AdminLayout.cshtml     # Layout chính
│   ├── Home/
│   │   └── Index.cshtml            # Dashboard
│   ├── Category/
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   └── Details.cshtml
│   └── Product/
│       ├── Index.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       └── Details.cshtml
└── Models/                         # ViewModels nếu cần
```

## Assets

```
wwwroot/
├── css/
│   └── admin.css                   # CSS cho Admin
├── js/
│   └── admin.js                    # JavaScript cho Admin
└── img/
    ├── products/                   # Thư mục lưu hình sản phẩm
    └── no-image.png               # Hình mặc định
```

## Phân quyền

### Administrator
- Truy cập đầy đủ tất cả chức năng
- Quản lý người dùng và phân quyền
- Xem báo cáo và thống kê
- Cấu hình hệ thống

### Editor
- Quản lý sản phẩm và danh mục
- Quản lý đơn hàng
- Quản lý cửa hàng
- Không thể quản lý người dùng

## URLs và Routes

```
/Admin                              # Dashboard
/Admin/Category                     # Quản lý danh mục
/Admin/Product                      # Quản lý sản phẩm
/Admin/Order                        # Quản lý đơn hàng
/Admin/Store                        # Quản lý cửa hàng
/Admin/User                         # Quản lý người dùng
```

## Công nghệ sử dụng

- **Backend**: ASP.NET Core MVC
- **Frontend**: Bootstrap 5, Font Awesome 6
- **Charts**: Chart.js
- **CSS**: Custom Admin theme
- **JavaScript**: jQuery, Bootstrap JS

## Hướng dẫn sử dụng

### 1. Truy cập Admin Panel
- Đăng nhập với tài khoản có quyền Administrator hoặc Editor
- Truy cập `/Admin` để vào dashboard

### 2. Navigation
- Click vào các menu để mở/đóng submenu
- Đường line sẽ hiện ra khi mở submenu
- Menu active sẽ được highlight

### 3. CRUD Operations
- **Create**: Click nút "Thêm mới" màu xanh lá
- **Read**: Click biểu tượng mắt để xem chi tiết
- **Update**: Click biểu tượng bút chì để chỉnh sửa
- **Delete**: Click biểu tượng thùng rác (có confirm)

### 4. Search & Filter
- Sử dụng search box để tìm kiếm
- Sử dụng dropdown để lọc theo danh mục
- Pagination tự động

### 5. Image Upload
- Drag & drop hoặc click để chọn file
- Preview ngay sau khi chọn
- Hỗ trợ JPG, PNG, GIF

## Customization

### Thay đổi màu sắc
Chỉnh sửa trong `admin.css`:
```css
:root {
    --admin-primary: #3498db;
    --admin-success: #2ecc71;
    --admin-warning: #f39c12;
    --admin-danger: #e74c3c;
}
```

### Thêm menu mới
1. Thêm menu item trong `_AdminLayout.cshtml`
2. Tạo controller và views tương ứng
3. Cập nhật permissions nếu cần

## Security

- Phân quyền dựa trên Roles
- CSRF protection
- XSS protection
- File upload validation
- SQL injection prevention

## Performance

- Lazy loading cho hình ảnh
- Pagination cho danh sách lớn
- Minified CSS/JS
- Optimized database queries
- Caching cho static content

---

*Phát triển bởi Toocha Development Team* 