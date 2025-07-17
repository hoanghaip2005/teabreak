# Tóm tắt Hệ thống Xác thực & Phân quyền - Toocha Tea

## Kiến trúc tổng quan

Hệ thống sử dụng **ASP.NET Core Identity** với kiến trúc phân tầng rõ ràng:

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                       │
├─────────────────────────────────────────────────────────────┤
│  Areas/Admin    │  Areas/Identity  │  Areas/Toocha         │
│  - UserMgmt     │  - Account       │  - Seller             │
│  - Roles        │  - Manage        │  - Order              │
│  - Content      │  - Roles         │  - Public             │
├─────────────────────────────────────────────────────────────┤
│                    BUSINESS LOGIC                           │
├─────────────────────────────────────────────────────────────┤
│  Controllers → Services → UserManager/RoleManager          │
├─────────────────────────────────────────────────────────────┤
│                     DATA LAYER                             │
├─────────────────────────────────────────────────────────────┤
│  AppDbContext → SQL Server → Identity Tables               │
└─────────────────────────────────────────────────────────────┘
```

## Các thành phần chính

### 1. Authentication (Xác thực)

#### Traditional Login:
- **Route**: `/login/`
- **Controller**: `AccountController`
- **Features**:
  - Email hoặc Username đăng nhập
  - Password validation
  - Account lockout protection
  - Two-Factor Authentication (2FA)
  - Remember me functionality

#### External Authentication:
- **Google OAuth**: 
  - Callback: `/dang-nhap-tu-google`
  - Cấu hình: `appsettings.json`
- **Facebook OAuth**: 
  - Callback: `/dang-nhap-tu-facebook` 
  - Cấu hình: `appsettings.json`

#### Session Management:
- **Cookie name**: `.AspNetCore.Identity.Application`
- **Expiry**: 7 ngày với sliding expiration
- **Security**: HttpOnly, Secure (production), SameSite=Lax

### 2. Authorization (Phân quyền)

#### Role Hierarchy:
```
Administrator (Toàn quyền)
├── Editor (Quản lý nội dung)
├── Manager (Giám sát kinh doanh)
├── Seller (Bán hàng)
└── Member (Cơ bản)

Workflow Roles:
├── ITManager → ITStaff
├── FinanceManager → FinanceStaff  
├── HRManager → HRStaff
└── ProcurementManager → ProcurementStaff
```

#### Access Control Matrix:

| Area/Feature | Admin | Editor | Manager | Seller | Member |
|--------------|-------|--------|---------|--------|--------|
| Admin Dashboard | ✅ | ❌ | ❌ | ❌ | ❌ |
| User Management | ✅ | ❌ | ❌ | ❌ | ❌ |
| Role Management | ✅ | ❌ | ❌ | ❌ | ❌ |
| Content Management | ✅ | ✅ | ❌ | ❌ | ❌ |
| Seller Dashboard | ✅ | ❌ | ✅ | ✅ | ❌ |
| Order Management | ✅ | ❌ | ✅ | ✅ | ❌ |
| Profile Management | ✅ | ✅ | ✅ | ✅ | ✅ |

### 3. Security Features

#### Protection Mechanisms:
- **CSRF Protection**: Anti-forgery tokens trên tất cả forms
- **Input Validation**: Model validation với data annotations
- **XSS Protection**: Automatic HTML encoding
- **SQL Injection**: Entity Framework parameter binding

#### Password Policy:
- Minimum length requirements
- Character complexity
- Account lockout after failed attempts
- Password reset via email

## Routes & Endpoints

### Authentication Routes:
```
/login/                    - Trang đăng nhập
/logout/                   - Đăng xuất
/Account/Register          - Đăng ký
/Account/ForgotPassword    - Quên mật khẩu
/Account/ExternalLogin     - Đăng nhập external
/dang-nhap-tu-google       - Google callback
/dang-nhap-tu-facebook     - Facebook callback
```

### Authorization Routes:
```
/Admin/{controller}/{action}     - Admin area (Administrator only)
/Member/{action}                 - Profile management
/ManageUser/{action}             - User management (Admin only)
/Role/{action}                   - Role management (Admin only)
/khongduoctruycap.html          - Access denied page
```

## Database Schema

### Core Identity Tables:
- **AspNetUsers**: Thông tin người dùng (extends với AppUser)
- **AspNetRoles**: Định nghĩa roles
- **AspNetUserRoles**: Mapping user-role (many-to-many)
- **AspNetUserClaims**: Claims của user
- **AspNetRoleClaims**: Claims của role
- **AspNetUserLogins**: External login mappings

### Custom Extensions:
```csharp
AppUser : IdentityUser
{
    string HomeAddress
    string Avatar  
    DateTime BirthDate
}
```

## Configuration

### Program.cs Setup:
```csharp
// Identity configuration
services.AddDefaultIdentity<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Authentication providers
services.AddAuthentication()
    .AddGoogle(options => { ... })
    .AddFacebook(options => { ... });

// Authorization policies
services.AddAuthorization(options => {
    options.AddPolicy("ViewManageMenu", builder => {
        builder.RequireAuthenticatedUser();
        builder.RequireRole(RoleName.Administrator);
    });
});
```

## Best Practices Implemented

### 1. Security:
- ✅ Separation of concerns (Areas)
- ✅ Role-based authorization
- ✅ Claims-based authorization
- ✅ External authentication integration
- ✅ Secure cookie configuration
- ✅ CSRF protection

### 2. User Experience:
- ✅ Remember me functionality
- ✅ External login options
- ✅ Account management features
- ✅ Two-factor authentication
- ✅ Password recovery

### 3. Code Organization:
- ✅ Area-based structure
- ✅ Consistent naming conventions
- ✅ Centralized role definitions
- ✅ Reusable authorization attributes
- ✅ Proper error handling

## Files Generated

1. **`authentication_authorization_activity_diagram.puml`**
   - Diagram chi tiết hoàn chỉnh
   - Mô tả tất cả luồng xử lý
   - Phù hợp cho developers

2. **`auth_simplified_diagram.puml`**  
   - Diagram đơn giản hóa
   - Luồng chính dễ hiểu
   - Phù hợp cho stakeholders

3. **`README_Auth_Diagrams.md`**
   - Hướng dẫn sử dụng PlantUML
   - Cách tạo hình ảnh từ code

4. **`auth_system_summary.md`** (file này)
   - Tóm tắt kiến trúc hệ thống
   - Phân tích chi tiết các thành phần

---

*Phân tích và tạo diagram cho hệ thống Toocha Tea - Cửa hàng trà sữa* 