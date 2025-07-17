# Hướng Dẫn Nhanh - Class Diagram Toocha

## Các File Đã Tạo

### 1. `toocha_class_diagram.puml`
- **Mô tả**: Class diagram đầy đủ với tất cả các class và mối quan hệ
- **Sử dụng khi**: Cần hiểu toàn bộ cấu trúc hệ thống
- **Độ phức tạp**: Cao

### 2. `toocha_simple_class_diagram.puml`
- **Mô tả**: Class diagram đơn giản với các class chính
- **Sử dụng khi**: Cần hiểu nhanh cấu trúc cơ bản
- **Độ phức tạp**: Thấp

### 3. `README_Class_Diagram_Guide.md`
- **Mô tả**: Hướng dẫn chi tiết cách vẽ class diagram
- **Sử dụng khi**: Học cách tạo class diagram
- **Độ phức tạp**: Trung bình

## Cách Sử Dụng

### Bước 1: Xem Class Diagram
1. Mở file `.puml` trong PlantUML editor
2. Hoặc sử dụng online editor: http://www.plantuml.com/plantuml/
3. Copy nội dung file và paste vào editor

### Bước 2: Export Diagram
- **PNG**: Chất lượng cao, dễ chia sẻ
- **SVG**: Vector, có thể zoom không mất chất lượng
- **PDF**: Phù hợp cho tài liệu

### Bước 3: Chỉnh Sửa
- Thay đổi màu sắc trong `skinparam`
- Thêm/bớt class theo nhu cầu
- Điều chỉnh mối quan hệ

## Cấu Trúc Hệ Thống Toocha

### Core Entities
```
AppUser (Người dùng)
├── Order (Đơn hàng)
│   └── OrderItem (Chi tiết đơn hàng)
└── ProductReview (Đánh giá)

Category (Danh mục)
└── Product (Sản phẩm)

Store (Cửa hàng)
└── Order (Đơn hàng)

Discount (Khuyến mãi)
├── Product (Sản phẩm)
└── Category (Danh mục)
```

### Mối Quan Hệ Chính
- **1-nhiều**: Category → Product, Order → OrderItem
- **Nhiều-1**: Product → Category, OrderItem → Order
- **Nhiều-nhiều**: OrderItem ↔ Topping (qua OrderItemTopping)

## Lưu Ý Quan Trọng

1. **AppUser** kế thừa từ `IdentityUser` của ASP.NET Core
2. **AppDbContext** kế thừa từ `IdentityDbContext<AppUser>`
3. **OrderItem** hỗ trợ tùy chỉnh (đường, đá, topping)
4. **Discount** có computed properties để validation
5. **Store** có thông tin địa lý (lat/lng)

## Công Cụ Khuyến Nghị

### Online
- [PlantUML Online](http://www.plantuml.com/plantuml/)
- [Draw.io](https://app.diagrams.net/)

### Desktop
- Visual Studio (có extension PlantUML)
- IntelliJ IDEA (có plugin PlantUML)
- VS Code (có extension PlantUML)

## Ví Dụ Sử Dụng

### Tạo Class Mới
```plantuml
class NewClass {
    +int Id
    +string Name
    +DateTime CreatedAt
    --
    +SomeMethod()
}
```

### Thêm Mối Quan Hệ
```plantuml
ExistingClass ||--o{ NewClass : relates_to
```

## Hỗ Trợ

Nếu cần hỗ trợ:
1. Đọc `README_Class_Diagram_Guide.md` để hiểu chi tiết
2. Tham khảo PlantUML documentation
3. Kiểm tra code trong thư mục `Models/` để hiểu cấu trúc thực tế 