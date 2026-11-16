# Báo cáo kiểm thử — Member 5

## Thông tin lần chạy

| Hạng mục | Kết quả |
|---|---|
| Ngày chạy | 2026-11-16 |
| Framework | xUnit trên .NET 10 (`net10.0-windows`) |
| Phạm vi | `AuthService` và `HashService` |
| Dữ liệu | EF Core InMemory, dữ liệu giả tạo riêng cho từng test |
| SQL Server thật | Không kết nối, không thay đổi dữ liệu |
| Tổng số | 13 |
| Passed | 13 |
| Failed | 0 |
| Skipped | 0 |

## Chi tiết test case

| # | Test case | Kết quả | Ghi chú |
|---:|---|---|---|
| 1 | Đăng nhập thành công; email được trim và chuẩn hóa chữ hoa/thường | Passed | Tạo session và trả đúng vai trò/tên hiển thị |
| 2 | Bỏ trống email | Passed | Từ chối, không sửa số lần đăng nhập sai |
| 3 | Bỏ trống mật khẩu | Passed | Từ chối, không sửa số lần đăng nhập sai |
| 4 | Nhập sai mật khẩu | Passed | Tăng số lần sai lên 1, chưa khóa tài khoản |
| 5 | Sai mật khẩu 5 lần liên tiếp | Passed | Khóa tài khoản; lần đăng nhập tiếp theo bị từ chối |
| 6 | Tài khoản ở trạng thái Inactive | Passed | Từ chối đăng nhập |
| 7 | Tài khoản đã khóa | Passed | Từ chối đăng nhập |
| 8 | Tài khoản đã xóa mềm | Passed | Từ chối đăng nhập |
| 9 | Gọi đăng nhập yêu cầu Coach bằng tài khoản Member | Passed | Từ chối sai vai trò, không tăng số lần sai mật khẩu |
| 10 | Email không tồn tại | Passed | Từ chối và không tạo session |
| 11 | Xác minh mật khẩu BCrypt đúng và sai | Passed | Đúng mật khẩu được chấp nhận, sai mật khẩu bị từ chối |
| 12 | Xác minh mật khẩu SHA-512 cũ | Passed | Kiểm tra tương thích dữ liệu mật khẩu cũ |
| 13 | Mật khẩu hoặc hash rỗng | Passed | Từ chối đầu vào rỗng |

## Nhận xét của tester

Các kiểm tra tự động trên tập trung vào quy tắc đăng nhập và xác minh hash. Đây là test tầng dịch vụ, **chưa phải E2E giao diện**: chưa tự mở WPF, nhập thông tin vào cửa sổ đăng nhập, hoặc xác nhận chuyển màn hình theo vai trò. Test cũng dùng dữ liệu giả, vì vậy chưa kiểm chứng kết nối SQL Server thật.

## Chạy lại

Mở PowerShell tại thư mục gốc repository và chạy:

```powershell
dotnet test .\SportCenterManament_PRN\SportCenterManament_PRN.slnx
```

Lần chạy thành công đã báo `Passed: 13, Failed: 0, Skipped: 0`. Build ứng dụng còn hai cảnh báo có sẵn ở `ManagerMainWindow.xaml.cs` và `CourseManagementView.xaml.cs`; chúng không làm các test trên thất bại.

> Ghi chú phiên làm việc: những lần chạy lại sau đó trong môi trường này dừng ở dòng “A total of 1 test files matched” mà không kết thúc test host. Vì vậy, kết quả 13/13 ở trên là lần chạy thành công đã quan sát được; cần chạy lại lệnh ở máy bạn để xác nhận trạng thái hiện tại trước khi dùng báo cáo như kết quả nghiệm thu cuối cùng.
