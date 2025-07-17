# 🏪 Sơ đồ Use Case Hệ thống Toocha Tea & Ice Cream

## Tổng quan

Hệ thống Toocha là một ứng dụng web quản lý cửa hàng trà sữa được phát triển bằng ASP.NET Core 9.0 với kiến trúc Multi-Area. Sơ đồ use case này mô tả toàn bộ 76 chức năng chính của hệ thống được phân chia thành 10 module và 4 nhóm người dùng chính, bao gồm các mối quan hệ include/extend để thể hiện sự phụ thuộc giữa các use case.

## Cách sử dụng sơ đồ

### 1. Xem sơ đồ trực tuyến (Khuyến nghị)
- Truy cập: [PlantUML Online Server](https://www.plantuml.com/plantuml/uml/)
- Copy nội dung file `toocha_complete_usecase_diagram.puml` và paste vào
- Click "Submit" để xem sơ đồ

### 2. Sử dụng Visual Studio Code
- Cài đặt extension "PlantUML"
- Mở file `toocha_complete_usecase_diagram.puml`
- Nhấn `Alt + D` hoặc `Ctrl+Shift+P` → "PlantUML: Preview Current Diagram"

### 3. Xuất hình ảnh
```bash
# Xuất PNG
java -jar plantuml.jar -tpng toocha_complete_usecase_diagram.puml

# Xuất SVG
java -jar plantuml.jar -tsvg toocha_complete_usecase_diagram.puml
```

## Kiến trúc hệ thống

### 🏗️ Công nghệ sử dụng
- **Framework**: ASP.NET Core 9.0 MVC
- **Database**: SQL Server với Entity Framework Core
- **Authentication**: ASP.NET Core Identity + External Login
- **Architecture**: Multi-Area (Admin, Identity, Toocha, Database)

### 📁 Cấu trúc Areas
```
Areas/
├── Admin/          # Quản lý hệ thống
├── Identity/       # Xác thực và phân quyền
├── Toocha/         # Giao diện người dùng
└── Database/       # Quản lý cơ sở dữ liệu
```

## Phân tích Actors (Nhóm người dùng)

### 👤 Khách vãng lai (Guest)
- **Số lượng Use Case**: 15
- **Quyền hạn**: 
  - Xem sản phẩm và thông tin cửa hàng
  - Đăng ký tài khoản mới
  - Xem nội dung công khai (tin tức, tuyển dụng)
- **Hạn chế**: Không thể đặt hàng hoặc thanh toán

### 👤 Khách hàng (Customer)
- **Số lượng Use Case**: 32
- **Quyền hạn**:
  - Tất cả quyền của Guest
  - Đặt hàng và thanh toán (COD, chuyển khoản, online)
  - Quản lý thông tin cá nhân
  - Xem lịch sử đơn hàng
  - Viết đánh giá sản phẩm

### 👨‍💼 Nhân viên bán hàng (Seller)
- **Số lượng Use Case**: 10
- **Quyền hạn**:
  - Xử lý và quản lý đơn hàng
  - Cập nhật trạng thái đơn hàng
  - Xem dashboard bán hàng cá nhân
  - Thống kê doanh số

### 👑 Quản trị viên (Administrator)
- **Số lượng Use Case**: 62
- **Quyền hạn**:
  - Toàn quyền quản lý hệ thống
  - Quản lý người dùng và phân quyền
  - Quản lý cơ sở dữ liệu
  - Backup/Restore dữ liệu
  - Quản lý sản phẩm và danh mục
  - Quản lý cửa hàng và vị trí
  - Quản lý nội dung (tin tức, sự kiện)
  - Quản lý khuyến mãi và mã giảm giá
  - Xem báo cáo tổng quan và thống kê
  - Quản lý tồn kho
  - Trả lời đánh giá khách hàng

## Phân tích Modules (Nhóm chức năng)

### 🔐 Xác thực & Quản lý người dùng (10 Use Cases)
- **UC001-UC010**: Đăng ký, đăng nhập, quên mật khẩu, quản lý profile
- **Tính năng nổi bật**: 
  - Social Login (Google, Facebook)
  - Email xác nhận
  - Phân quyền dựa trên Role

### 📦 Quản lý sản phẩm & Danh mục (11 Use Cases)
- **UC011-UC021**: CRUD sản phẩm, danh mục, size, topping
- **Tính năng nổi bật**:
  - Tìm kiếm và lọc sản phẩm
  - Quản lý tồn kho
  - Upload hình ảnh

### 🛒 Quản lý đơn hàng & Giỏ hàng (12 Use Cases)
- **UC022-UC033**: Đặt hàng, thanh toán, theo dõi trạng thái
- **Tính năng nổi bật**:
  - Tùy chỉnh sản phẩm (Size, Topping, Đá, Đường)
  - Đa phương thức thanh toán
  - Theo dõi real-time

### 🏬 Quản lý cửa hàng & Giao hàng (8 Use Cases)
- **UC034-UC041**: Quản lý cửa hàng, tính phí giao hàng
- **Tính năng nổi bật**:
  - Tích hợp Google Maps
  - Tìm cửa hàng gần nhất
  - Tính phí giao hàng thông minh

### 🎁 Quản lý khuyến mãi & Giảm giá (7 Use Cases)
- **UC042-UC048**: Mã giảm giá, coupon, quà tặng
- **Tính năng nổi bật**:
  - Nhiều loại khuyến mãi
  - Theo dõi hiệu quả
  - Áp dụng tự động

### 📊 Báo cáo & Thống kê (7 Use Cases)
- **UC049-UC055**: Dashboard, báo cáo doanh thu, thống kê
- **Tính năng nổi bật**:
  - Biểu đồ realtime
  - Xuất báo cáo
  - Thống kê đa chiều

### 📝 Quản lý nội dung (9 Use Cases)
- **UC056-UC064**: Tin tức, tuyển dụng, nhượng quyền, sự kiện
- **Tính năng nổi bật**:
  - CMS tích hợp
  - Quản lý đa loại nội dung
  - SEO friendly

### 🗄️ Quản lý cơ sở dữ liệu (4 Use Cases)
- **UC065-UC068**: Seed data, backup/restore, migration
- **Tính năng nổi bật**:
  - Tự động backup
  - Migration control
  - Data seeding

### ⭐ Đánh giá & Nhận xét (4 Use Cases)
- **UC069-UC072**: Đánh giá sản phẩm, quản lý feedback
- **Tính năng nổi bật**:
  - Đánh giá 5 sao
  - Trả lời đánh giá
  - Lọc theo rating

### 👨‍💼 Dashboard Nhân viên (4 Use Cases)
- **UC073-UC076**: Dashboard chuyên dụng cho seller
- **Tính năng nổi bật**:
  - Thống kê cá nhân
  - Top sản phẩm
  - Đơn hàng gần đây

## Mối quan hệ Include/Extend

### 🔗 Include Relationships (Phụ thuộc bắt buộc)
Các use case này luôn cần thiết khi thực hiện use case chính:

| Use Case Chính | Include | Mô tả |
|----------------|---------|-------|
| Đăng ký tài khoản | Đăng nhập | Sau khi đăng ký thành công |
| Đặt hàng | Đăng nhập | Yêu cầu đăng nhập trước |
| Quản lý đơn hàng | Đăng nhập | Yêu cầu quyền admin/seller |
| Cập nhật trạng thái | Đăng nhập | Yêu cầu quyền xử lý |
| Dashboard | Đăng nhập | Yêu cầu xác thực |
| Đặt hàng | Gửi email | Gửi email xác nhận |
| Quên mật khẩu | Gửi email | Gửi email reset |
| Đặt hàng | Tính phí giao hàng | Tính phí tự động |
| Tìm cửa hàng gần nhất | Tính phí giao hàng | Tính phí theo khoảng cách |

### 🔀 Extend Relationships (Mở rộng tùy chọn)
Các use case này có thể mở rộng thêm chức năng:

| Use Case Mở rộng | Extend từ | Mô tả |
|------------------|-----------|-------|
| Áp dụng mã giảm giá | Đặt hàng | Tùy chọn sử dụng mã giảm giá |
| Tùy chỉnh sản phẩm | Thêm vào giỏ hàng | Tùy chỉnh size, topping |
| Viết đánh giá | Theo dõi đơn hàng | Sau khi nhận hàng |
| Xem bản đồ | Xem cửa hàng | Xem vị trí trên bản đồ |
| Thanh toán chuyển khoản | Chọn phương thức | Một trong các phương thức |
| Thanh toán online | Chọn phương thức | Một trong các phương thức |

## Tích hợp hệ thống ngoài

### 💳 Hệ thống thanh toán (Payment Gateway)
- **Liên kết**: UC026, UC029
- **Chức năng**: Xử lý thanh toán online, xác nhận giao dịch

### 🗺️ Google Maps API
- **Liên kết**: UC035, UC036, UC037, UC039
- **Chức năng**: Hiển thị bản đồ, tính khoảng cách, geocoding

### 📧 Email Service
- **Liên kết**: UC004, UC005, UC025
- **Chức năng**: Gửi email xác nhận, reset password, thông báo đơn hàng

### 📱 Social Login
- **Liên kết**: UC003
- **Chức năng**: Đăng nhập qua Google, Facebook

## Luồng nghiệp vụ chính

### 1. Quy trình đặt hàng
```
Guest → Đăng ký → Customer → Chọn sản phẩm → Tùy chỉnh → 
Thêm giỏ hàng → Đặt hàng → Chọn thanh toán → Hoàn thành
```

### 2. Quy trình xử lý đơn hàng
```
Đơn hàng mới → Seller xử lý → Cập nhật trạng thái → 
Giao hàng → Hoàn thành → Đánh giá
```

### 3. Quy trình quản lý sản phẩm
```
Editor → Tạo danh mục → Thêm sản phẩm → Upload hình ảnh → 
Cập nhật tồn kho → Publish
```

## Metrics & KPI

### Phân bố Use Case theo Actor
| Actor | Số Use Cases | Tỷ lệ |
|-------|-------------|-------|
| Administrator | 62 | 81.6% |
| Customer | 32 | 42.1% |
| Guest | 15 | 19.7% |
| Seller | 10 | 13.2% |

### Mối quan hệ Include/Extend
| Loại quan hệ | Số lượng | Mô tả |
|-------------|----------|-------|
| Include | 9 | Phụ thuộc bắt buộc |
| Extend | 6 | Mở rộng tùy chọn |

### Phân bố Use Case theo Module
| Module | Số Use Cases | Tỷ lệ |
|--------|-------------|-------|
| Quản lý đơn hàng | 12 | 15.8% |
| Quản lý sản phẩm | 11 | 14.5% |
| Xác thực & User | 10 | 13.2% |
| Quản lý nội dung | 9 | 11.8% |
| Quản lý cửa hàng | 8 | 10.5% |
| Khuyến mãi | 7 | 9.2% |
| Báo cáo | 7 | 9.2% |
| Dashboard Seller | 4 | 5.3% |
| Đánh giá | 4 | 5.3% |
| Database | 4 | 5.3% |

## Tính năng nổi bật

### ✅ Smart Features
- **Smart Shipping**: Tự động tìm cửa hàng gần nhất và tính phí giao hàng
- **Product Customization**: Tùy chỉnh linh hoạt size, topping, đá, đường
- **Multi-payment**: Hỗ trợ COD, chuyển khoản, thanh toán online
- **Social Integration**: Đăng nhập qua Google, Facebook

### ✅ Management Features
- **Role-based Access**: Phân quyền chi tiết theo vai trò
- **Real-time Analytics**: Dashboard và báo cáo thời gian thực
- **Content Management**: CMS tích hợp quản lý nội dung
- **Inventory Management**: Quản lý tồn kho thông minh

### ✅ Business Features
- **Promotion Engine**: Hệ thống khuyến mãi đa dạng
- **Review System**: Đánh giá và feedback khách hàng
- **Store Locator**: Tìm cửa hàng với Google Maps
- **Order Tracking**: Theo dõi đơn hàng real-time

## Hướng dẫn triển khai

### 1. Cài đặt môi trường
```bash
# Clone repository
git clone https://github.com/hoanghaip2005/teabreak.git

# Cài đặt dependencies
dotnet restore

# Cập nhật database
dotnet ef database update
```

### 2. Cấu hình
- Cập nhật connection string trong `appsettings.json`
- Cấu hình Google/Facebook OAuth
- Cấu hình SMTP server cho email

### 3. Chạy ứng dụng
```bash
dotnet run
```

### 4. Truy cập các area
- **Public**: `/` - Trang chủ Toocha
- **Admin**: `/Admin` - Dashboard quản trị
- **Identity**: `/Account` - Quản lý tài khoản
- **Database**: `/Database` - Quản lý CSDL

## Liên hệ & Hỗ trợ

- **Repository**: [GitHub - teabreak](https://github.com/hoanghaip2005/teabreak)
- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server
- **Version**: 1.0.0

---

*Sơ đồ này được tạo dựa trên phân tích toàn bộ source code của hệ thống Toocha và phản ánh đúng các chức năng hiện có trong hệ thống.* 