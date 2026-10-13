# Sổ Sách — Web app MVC quản lý sách

Ứng dụng ASP.NET Core MVC (.NET 10), Entity Framework Core, **Code First**, SQLite. Có thể cấu hình dùng SQL Server.

## Chạy ứng dụng

Cài .NET 10 SDK, mở PowerShell trong thư mục bài 6:

```powershell
cd BookManager
dotnet restore
dotnet run --no-launch-profile --urls http://localhost:5186
```

Mở http://localhost:5186. Lần chạy đầu tự áp dụng migration, tạo `books.db` và 5 sách mẫu. Dữ liệu CRUD được lưu thật trong CSDL và tồn tại sau khi khởi động lại. Chạy lệnh từ thư mục `BookManager` để sử dụng cùng đường dẫn CSDL.

## Chức năng

- Danh sách và chi tiết sách.
- Thêm, sửa, xóa (xác nhận trước khi xóa).
- Tìm kiếm theo tên/tác giả, lọc thể loại, phân trang 8 sách/trang.
- Thống kê đầu sách, số lượng và thể loại.
- Kiểm tra dữ liệu bắt buộc, độ dài, năm, giá và số lượng tại server.
- POST kèm anti-forgery token; GET không thay đổi dữ liệu.
- Giao diện tiếng Việt, hiển thị trên máy tính và điện thoại.

## Kiến trúc MVC / Code First

```text
BookManager/
├── Models/Book.cs               # Entity và view model
├── Data/LibraryContext.cs       # DbContext, ánh xạ, dữ liệu mẫu
├── Controllers/BooksController.cs # Điều phối CRUD bằng EF Core
├── Views/Books/                 # Razor: Index, Details, Create, Edit, Delete
├── Views/Shared/_Layout.cshtml  # Bố cục giao diện
├── Migrations/                 # Lược đồ SQLite từ mô hình C#
├── wwwroot/css/site.css         # Giao diện responsive
├── Program.cs                  # DI, provider, tạo CSDL, route
└── appsettings.json            # Cấu hình kết nối
```

Model `Book` có Id (khóa chính tự tăng), Title, Author, Category, Publisher, PublishedYear, Price, Quantity, Description. Controller đọc/ghi qua `LibraryContext`; View hiển thị dữ liệu bằng Razor. EF Core tạo bảng từ model C#, nên đây là Code First.

## Chuyển sang SQL Server

1. Cài và chạy SQL Server/LocalDB. Ứng dụng đã có gói EF Core SqlServer.
2. Trong `BookManager/appsettings.json`, đặt `DatabaseProvider` thành `SqlServer`.
3. Sửa `ConnectionStrings:SqlServer` theo máy, ví dụ:

```json
"SqlServer": "Server=(localdb)\\MSSQLLocalDB;Database=BookManager;Trusted_Connection=True;TrustServerCertificate=True"
```

4. Chạy lại ứng dụng. Với SQL Server, ứng dụng dùng `EnsureCreated()` để tạo CSDL/bảng từ model và thêm dữ liệu mẫu khi CSDL mới; tài khoản phải có quyền tạo CSDL. Không sử dụng migration SQLite cho SQL Server. Hai CSDL độc lập, đổi provider không tự chuyển dữ liệu giữa chúng.

Nếu cần thay đổi lược đồ SQL Server sau khi đã tạo, hãy tạo bộ migration riêng cho provider này trước khi triển khai. Không lưu mật khẩu thật vào mã nguồn; có thể cung cấp chuỗi kết nối qua biến môi trường `ConnectionStrings__SqlServer`.

## Cập nhật mô hình SQLite

```powershell
# Tại thư mục bài 6, nếu chưa có công cụ cục bộ
# dotnet tool install dotnet-ef --tool-path .tools --version 10.0.0
.\.tools\dotnet-ef migrations add TenThayDoi --project BookManager
.\.tools\dotnet-ef database update --project BookManager
```

## Kiểm tra

```powershell
dotnet build BookManager
# Chạy ứng dụng trước, rồi mở PowerShell khác:
.\tests\crud-smoke.ps1
```

Ứng dụng phù hợp bài thực hành quản lý sách. Bản này chưa triển khai chức năng đăng nhập/phân quyền.

