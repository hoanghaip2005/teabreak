# 🏪 Sơ đồ Use Case Đơn giản - Hệ thống Toocha Tea & Ice Cream

## Tổng quan

Đây là phiên bản đơn giản hóa của sơ đồ use case hệ thống Toocha, tập trung vào **27 chức năng cốt lõi** thay vì 76 chức năng đầy đủ. Sơ đồ này dễ hiểu hơn và phù hợp cho việc trình bày với stakeholders không chuyên về kỹ thuật.

## Cách sử dụng sơ đồ

### Xem sơ đồ trực tuyến
- Truy cập: [PlantUML Online Server](https://www.plantuml.com/plantuml/uml/)
- Copy nội dung file `toocha_simplified_usecase_diagram.puml`
- Paste vào và click "Submit"

### Xuất hình ảnh
```bash
java -jar plantuml.jar -tpng toocha_simplified_usecase_diagram.puml
```

## Cấu trúc đơn giản

### 👥 4 Actors chính
- **👤 Khách vãng lai (Guest)**: Xem sản phẩm, đăng ký
- **👤 Khách hàng (Customer)**: Đặt hàng, thanh toán
- **👨‍💼 Nhân viên (Seller)**: Xử lý đơn hàng
- **👑 Quản trị viên (Admin)**: Quản lý toàn bộ hệ thống

### 🔧 6 Modules cốt lõi
1. **🔐 Xác thực** (4 use cases)
2. **📦 Quản lý sản phẩm** (5 use cases)
3. **🛒 Quản lý đơn hàng** (7 use cases)
4. **🏬 Quản lý cửa hàng** (4 use cases)
5. **⚙️ Quản lý hệ thống** (4 use cases)
6. **🎁 Khuyến mãi** (3 use cases)

### 🌐 3 Hệ thống ngoài
- **💳 Thanh toán Online**: Xử lý thanh toán
- **🗺️ Google Maps**: Tìm cửa hàng, tính khoảng cách
- **📧 Email**: Gửi email xác nhận

## Danh sách Use Cases theo Module

### 🔐 Xác thực (4 use cases)
| ID | Use Case | Mô tả |
|---|----------|-------|
| UC001 | Đăng ký tài khoản | Tạo tài khoản mới |
| UC002 | Đăng nhập/Đăng xuất | Xác thực người dùng |
| UC003 | Quên mật khẩu | Reset mật khẩu qua email |
| UC004 | Quản lý thông tin cá nhân | Cập nhật profile |

### 📦 Quản lý sản phẩm (5 use cases)
| ID | Use Case | Mô tả |
|---|----------|-------|
| UC005 | Xem danh sách sản phẩm | Hiển thị tất cả sản phẩm |
| UC006 | Xem chi tiết sản phẩm | Thông tin chi tiết sản phẩm |
| UC007 | Tìm kiếm sản phẩm | Tìm kiếm theo từ khóa |
| UC008 | Quản lý sản phẩm | CRUD sản phẩm (Admin) |
| UC009 | Quản lý danh mục | CRUD danh mục (Admin) |

### 🛒 Quản lý đơn hàng (7 use cases)
| ID | Use Case | Mô tả |
|---|----------|-------|
| UC010 | Thêm sản phẩm vào giỏ hàng | Thêm sản phẩm |
| UC011 | Tùy chỉnh sản phẩm | Size, topping, đá, đường |
| UC012 | Đặt hàng | Tạo đơn hàng mới |
| UC013 | Thanh toán | COD/Chuyển khoản/Online |
| UC014 | Theo dõi đơn hàng | Xem trạng thái đơn hàng |
| UC015 | Quản lý đơn hàng | Xử lý đơn hàng (Seller/Admin) |
| UC016 | Cập nhật trạng thái đơn hàng | Thay đổi trạng thái |

### 🏬 Quản lý cửa hàng (4 use cases)
| ID | Use Case | Mô tả |
|---|----------|-------|
| UC017 | Xem danh sách cửa hàng | Hiển thị tất cả cửa hàng |
| UC018 | Tìm cửa hàng gần nhất | Tìm theo vị trí |
| UC019 | Quản lý cửa hàng | CRUD cửa hàng (Admin) |
| UC020 | Tính phí giao hàng | Tính phí theo khoảng cách |

### ⚙️ Quản lý hệ thống (4 use cases)
| ID | Use Case | Mô tả |
|---|----------|-------|
| UC021 | Quản lý người dùng | CRUD người dùng (Admin) |
| UC022 | Phân quyền | Gán roles cho user (Admin) |
| UC023 | Xem dashboard | Thống kê tổng quan |
| UC024 | Báo cáo doanh thu | Báo cáo doanh thu (Admin) |

### 🎁 Khuyến mãi (3 use cases)
| ID | Use Case | Mô tả |
|---|----------|-------|
| UC025 | Xem khuyến mãi | Hiển thị khuyến mãi |
| UC026 | Áp dụng mã giảm giá | Sử dụng mã giảm giá |
| UC027 | Quản lý khuyến mãi | CRUD khuyến mãi (Admin) |

## Mối quan hệ Include/Extend

### 🔗 Include (6 mối quan hệ)
| Use Case chính | Include | Lý do |
|----------------|---------|-------|
| Đăng ký → Đăng nhập | UC001 → UC002 | Tự động đăng nhập sau đăng ký |
| Đặt hàng → Đăng nhập | UC012 → UC002 | Yêu cầu đăng nhập |
| Quản lý đơn hàng → Đăng nhập | UC015 → UC002 | Yêu cầu quyền |
| Dashboard → Đăng nhập | UC023 → UC002 | Yêu cầu xác thực |
| Đặt hàng → Tính phí | UC012 → UC020 | Tự động tính phí |
| Tìm cửa hàng → Tính phí | UC018 → UC020 | Tính phí theo khoảng cách |

### 🔀 Extend (3 mối quan hệ)
| Use Case mở rộng | Extend từ | Điều kiện |
|------------------|-----------|-----------|
| Áp mã giảm giá → Đặt hàng | UC026 → UC012 | Tùy chọn có mã |
| Tùy chỉnh → Thêm giỏ hàng | UC011 → UC010 | Tùy chọn tùy chỉnh |
| Thanh toán → Đặt hàng | UC013 → UC012 | Chọn phương thức |

## Phân tích Actors

### 👤 Khách vãng lai (Guest) - 8 use cases
**Quyền hạn:**
- Xem sản phẩm và thông tin cửa hàng
- Tìm kiếm sản phẩm
- Xem khuyến mãi
- Đăng ký tài khoản

**Hạn chế:**
- Không thể đặt hàng
- Không thể thanh toán

### 👤 Khách hàng (Customer) - 16 use cases
**Quyền hạn:**
- Tất cả quyền của Guest
- Đặt hàng và thanh toán
- Quản lý thông tin cá nhân
- Theo dõi đơn hàng
- Tùy chỉnh sản phẩm

### 👨‍💼 Nhân viên (Seller) - 4 use cases
**Quyền hạn:**
- Xử lý đơn hàng
- Cập nhật trạng thái đơn hàng
- Xem dashboard bán hàng

### 👑 Quản trị viên (Admin) - 11 use cases
**Quyền hạn:**
- Quản lý sản phẩm và danh mục
- Quản lý đơn hàng
- Quản lý cửa hàng
- Quản lý người dùng và phân quyền
- Xem dashboard và báo cáo
- Quản lý khuyến mãi

## Luồng nghiệp vụ chính

### 1. Luồng mua hàng (Customer Journey)
```
👤 Guest → Xem sản phẩm → Đăng ký → 👤 Customer → 
Thêm giỏ hàng → Tùy chỉnh → Đặt hàng → 
Thanh toán → Theo dõi đơn hàng
```

### 2. Luồng xử lý đơn hàng (Order Processing)
```
Đơn hàng mới → 👨‍💼 Seller → Quản lý đơn hàng → 
Cập nhật trạng thái → Hoàn thành
```

### 3. Luồng quản lý sản phẩm (Product Management)
```
👑 Admin → Quản lý danh mục → Quản lý sản phẩm → 
Publish → Khách hàng xem được
```

## So sánh với sơ đồ đầy đủ

| Tiêu chí | Sơ đồ đầy đủ | Sơ đồ đơn giản | Cải thiện |
|----------|--------------|----------------|-----------|
| Số Use Cases | 76 | 27 | Giảm 64% |
| Số Actors | 4 | 4 | Giữ nguyên |
| Số Modules | 10 | 6 | Giảm 40% |
| Độ phức tạp | Cao | Thấp | Dễ hiểu hơn |
| Phù hợp với | Developers | Stakeholders | Rộng rãi hơn |

## Tính năng bị loại bỏ

### ❌ Đã loại bỏ để đơn giản hóa:
- **Quản lý nội dung**: Tin tức, sự kiện, nhượng quyền, tuyển dụng
- **Đánh giá sản phẩm**: Viết đánh giá, quản lý đánh giá
- **Báo cáo chi tiết**: Báo cáo sản phẩm bán chạy, thống kê phức tạp
- **Dashboard chuyên biệt**: Dashboard riêng cho seller
- **Quản lý database**: Seed data, backup/restore
- **Chức năng nâng cao**: Quản lý size, topping, tồn kho chi tiết

### ✅ Được gộp chung:
- **Thanh toán**: Gộp COD, chuyển khoản, online thành 1 use case
- **Quản lý sản phẩm**: Gộp CRUD, upload hình, ẩn/hiện
- **Dashboard**: Gộp dashboard admin và seller

## Khi nào sử dụng sơ đồ này?

### ✅ Phù hợp cho:
- **Trình bày với stakeholders** không chuyên kỹ thuật
- **Đào tạo người dùng mới** về hệ thống
- **Tài liệu tổng quan** dự án
- **Phân tích yêu cầu** ban đầu

### ❌ Không phù hợp cho:
- **Phát triển chi tiết** hệ thống
- **Testing** đầy đủ chức năng
- **Tài liệu kỹ thuật** chi tiết
- **Phân tích business** sâu

## Kết luận

Sơ đồ use case đơn giản này giúp:
- **Dễ hiểu hơn** cho người không chuyên
- **Tập trung vào chức năng cốt lõi** của hệ thống
- **Giảm độ phức tạp** mà vẫn đảm bảo tính đầy đủ
- **Phù hợp cho trình bày** và thảo luận

Đây là điểm khởi đầu tốt để hiểu hệ thống Toocha trước khi đi vào chi tiết các chức năng nâng cao.

---

*Sơ đồ này tập trung vào 27 chức năng cốt lõi nhất của hệ thống Toocha, đảm bảo tính đơn giản nhưng vẫn đầy đủ cho việc hiểu tổng quan.* 