# Activity Diagrams - Hệ thống Xác thực & Phân quyền Toocha Tea

## Tổng quan

Dự án này cung cấp các activity diagram mô tả hệ thống xác thực (authentication) và phân quyền (authorization) của ứng dụng Toocha Tea được xây dựng bằng ASP.NET Core Identity.

## Files được tạo

### 1. `authentication_authorization_activity_diagram.puml`
- **Mô tả**: Activity diagram chi tiết và hoàn chỉnh
- **Nội dung**: Mô tả toàn bộ quy trình từ đăng nhập đến phân quyền
- **Độ phức tạp**: Cao - Phù hợp cho developers và system architects

### 2. `auth_simplified_diagram.puml`
- **Mô tả**: Phiên bản đơn giản hóa, dễ hiểu
- **Nội dung**: Luồng chính của authentication và authorization
- **Độ phức tạp**: Thấp - Phù hợp cho business analysts và stakeholders

## Cách sử dụng

### Bước 1: Cài đặt PlantUML

#### Online (Khuyến nghị cho người mới):
- Truy cập: [PlantUML Online Server](https://www.plantuml.com/plantuml/uml/)
- Copy nội dung file `.puml` và paste vào online editor
- Xem kết quả ngay lập tức

#### Local Installation:
```bash
# Cài đặt Java (yêu cầu)
# Download PlantUML JAR file từ: https://plantuml.com/download

# Sử dụng via command line:
java -jar plantuml.jar authentication_authorization_activity_diagram.puml
```

#### Visual Studio Code Extension:
1. Cài đặt extension "PlantUML"
2. Mở file `.puml`
3. Sử dụng `Alt + D` để xem preview

### Bước 2: Tạo hình ảnh

```bash
# Tạo PNG
java -jar plantuml.jar -tpng authentication_authorization_activity_diagram.puml

# Tạo SVG
java -jar plantuml.jar -tsvg authentication_authorization_activity_diagram.puml

# Tạo PDF
java -jar plantuml.jar -tpdf authentication_authorization_activity_diagram.puml
```

## Cấu trúc hệ thống được mô tả

### 1. Authentication (Xác thực)

#### Đăng nhập thường:
- Email/Username + Password
- Kiểm tra định dạng và validation
- Two-Factor Authentication (2FA)
- Account lockout protection

#### External Login:
- Google OAuth (`/dang-nhap-tu-google`)
- Facebook OAuth (`/dang-nhap-tu-facebook`)
- External login confirmation
- Account linking

### 2. Authorization (Phân quyền)

#### Roles trong hệ thống:
- **Administrator**: Quản trị viên - Toàn quyền
- **Editor**: Biên tập viên - Quản lý nội dung
- **Manager**: Quản lý - Giám sát kinh doanh
- **Seller**: Nhân viên bán hàng - Xử lý đơn hàng
- **Member**: Thành viên - Quyền cơ bản

#### Roles mở rộng (Workflow):
- **ITStaff/ITManager**: Nhân viên/Quản lý IT
- **FinanceStaff/FinanceManager**: Nhân viên/Quản lý Tài chính
- **HRStaff/HRManager**: Nhân viên/Quản lý Nhân sự
- **ProcurementStaff/ProcurementManager**: Nhân viên/Quản lý Mua sắm

#### Areas & Controllers:
- **Admin Area**: Chỉ Administrator
- **Seller Area**: Administrator, Seller, Manager
- **Identity Area**: Tất cả user đã đăng nhập
- **Database Area**: Quản lý database

### 3. Security Features

#### Session Management:
- Cookie-based authentication
- 7 ngày expiry với sliding expiration
- Secure cookie trong production

#### Protection:
- CSRF protection với Anti-Forgery tokens
- Account lockout sau nhiều lần đăng nhập sai
- Secure password requirements

## Luồng chính được mô tả

1. **User Access**: Người dùng truy cập hệ thống
2. **Authentication Check**: Kiểm tra yêu cầu đăng nhập
3. **Login Process**: Xử lý đăng nhập (thường hoặc external)
4. **Authorization Check**: Kiểm tra quyền truy cập
5. **Role-based Access**: Phân quyền theo role
6. **Session Management**: Quản lý session và logout

## Customization

### Thêm role mới:
1. Cập nhật `Data/RoleName.cs`
2. Thêm role vào authorization attributes
3. Cập nhật diagram tương ứng

### Thêm external provider:
1. Cấu hình trong `Program.cs`
2. Thêm callback path
3. Cập nhật UI và logic xử lý

## Lưu ý khi sử dụng

- File PlantUML sử dụng tiếng Việt, đảm bảo encoding UTF-8
- Diagram có thể lớn, sử dụng zoom để xem chi tiết
- Phiên bản simplified phù hợp cho presentation
- Phiên bản chi tiết phù hợp cho documentation

## Support & Documentation

- **PlantUML Documentation**: [https://plantuml.com/](https://plantuml.com/)
- **ASP.NET Core Identity**: [Microsoft Docs](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- **Activity Diagram Syntax**: [PlantUML Activity Diagram](https://plantuml.com/activity-diagram-beta)

---

*Được tạo cho dự án Toocha Tea - Hệ thống quản lý cửa hàng trà sữa* 