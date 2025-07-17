# 🏪 Sơ đồ PlantUML - Hệ thống Toocha

## Mô tả
Bộ sưu tập các sơ đồ PlantUML mô tả toàn bộ hệ thống quản lý cửa hàng trà sữa Toocha, bao gồm use case, quy trình và trạng thái.

## Danh sách các file sơ đồ

### 1. `toocha_usecase_overview.puml`
**Sơ đồ Use Case Tổng quan**
- Mô tả cái nhìn tổng quan về hệ thống
- Bao gồm 4 actor chính và các chức năng chính
- Thể hiện tương tác với các hệ thống bên ngoài
- Phù hợp để trình bày cho stakeholder không kỹ thuật

### 2. `toocha_usecase_detailed.puml`
**Sơ đồ Use Case Chi tiết**
- Mô tả chi tiết tất cả 60 use case trong hệ thống
- Phân chia thành 8 module chính:
  - 🔐 Xác thực & Quản lý người dùng
  - 📦 Quản lý sản phẩm
  - 🛒 Quản lý đơn hàng
  - 🏬 Quản lý cửa hàng & Giao hàng
  - 🎁 Quản lý khuyến mãi
  - 📊 Báo cáo & Thống kê
  - 📝 Quản lý nội dung
  - 🗄️ Quản lý cơ sở dữ liệu
  - ⭐ Đánh giá sản phẩm
- Thể hiện quyền hạn của từng actor

### 3. `toocha_order_workflow.puml`
**Quy trình Đặt hàng**
- Activity diagram mô tả quy trình đặt hàng từ đầu đến cuối
- Bao gồm các bước:
  - Duyệt và chọn sản phẩm
  - Tùy chỉnh sản phẩm (size, topping, đá, đường)
  - Quản lý giỏ hàng
  - Thanh toán (COD, chuyển khoản, online)
  - Xử lý đơn hàng bởi nhân viên
  - Giao hàng và hoàn thành
- Thể hiện luồng xử lý song song giữa khách hàng, hệ thống và nhân viên

### 4. `toocha_order_status.puml`
**Sơ đồ Trạng thái Đơn hàng**
- State diagram mô tả vòng đời của một đơn hàng
- 6 trạng thái chính:
  - 🔄 **Pending** (Chờ xử lý)
  - ⚙️ **Processing** (Đang xử lý)
  - 🚚 **Shipped** (Đang giao)
  - ✅ **Delivered** (Đã giao)
  - ❌ **Cancelled** (Đã hủy)
  - ⚠️ **Failed** (Giao thất bại)
- Các chuyển đổi trạng thái được xác định rõ ràng
- Bao gồm note giải thích cho từng trạng thái

## Cách sử dụng

### 1. Online (khuyên dùng)
- Truy cập [PlantUML Online Server](http://www.plantuml.com/plantuml/uml/)
- Copy nội dung file .puml và paste vào
- Click "Submit" để render sơ đồ

### 2. Visual Studio Code
- Cài đặt extension "PlantUML"
- Mở file .puml
- Nhấn `Ctrl+Shift+P` → "PlantUML: Preview Current Diagram"

### 3. Local PlantUML
```bash
# Cài đặt Java và PlantUML
java -jar plantuml.jar filename.puml
```

## Thông tin hệ thống

### Công nghệ
- **Framework**: ASP.NET Core 9.0
- **Database**: SQL Server với Entity Framework Core
- **Authentication**: ASP.NET Core Identity + External Login
- **Architecture**: Multi-Area (Admin, Identity, Toocha)

### Vai trò người dùng
| Vai trò | Mô tả | Số lượng Use Case |
|---------|-------|-------------------|
| 👤 **Guest** | Khách vãng lai | 11 |
| 👤 **Customer** | Khách hàng đã đăng ký | 25 |
| 👨‍💼 **Seller** | Nhân viên bán hàng | 7 |
| 👨‍💼 **Manager** | Quản lý cửa hàng | 15 |
| ✏️ **Editor** | Biên tập viên | 18 |
| 👑 **Administrator** | Quản trị viên | 35 |

### Tính năng nổi bật
- ✅ Smart Shipping với tìm cửa hàng gần nhất
- ✅ Tùy chỉnh sản phẩm linh hoạt (size, topping, đường, đá)
- ✅ Đa phương thức thanh toán
- ✅ Phân quyền chi tiết theo vai trò
- ✅ Báo cáo và thống kê toàn diện
- ✅ Tích hợp Google Maps và Social Login
- ✅ Quản lý khuyến mãi và mã giảm giá

## Liên hệ
- **Dự án**: Hệ thống quản lý cửa hàng trà sữa Toocha
- **Phiên bản**: ASP.NET Core 9.0
- **Ngày tạo sơ đồ**: 2025

---
*Các sơ đồ này được tạo dựa trên phân tích toàn bộ source code của hệ thống Toocha* 