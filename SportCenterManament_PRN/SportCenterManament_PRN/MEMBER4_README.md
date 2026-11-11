# Member 4 – Điểm danh học viên

## Đã bổ sung vào project gốc
- `DataAccess/Entities/ClassAttendance.cs`: entity ánh xạ bảng `dbo.ClassAttendance`.
- `DataAccess/Entities/SportsCenterDbContext.cs`: DbSet và quan hệ EF Core.
- `ClassRegistration.cs`, `ClassSession.cs`, `Member.cs`: navigation properties.
- `Services/AttendanceService/AttendanceService.cs`: tải lịch, tải danh sách đã đăng ký, lưu điểm danh, báo cáo chuyên cần.
- `ViewModels/Attendance/`: các model cho màn hình.
- `Views/AttendanceWindow.xaml` và `.xaml.cs`: giao diện điểm danh WPF.

## Database
Bộ code này giả định bảng `dbo.ClassAttendance` đã tồn tại trong `SportsCenterManagement` với các cột:
`Id`, `ClassRegistrationId`, `SessionId`, `MemberId`, `IsPresent`, `Notes`, `MarkedBy`, `MarkedAt`.
Không cần chạy lại SQL tạo bảng nếu bảng đã có.

Project gốc đang cấu hình connection string trong `appsettings.json` là:
`Server=.\SQLEXPRESS01;Database=SportsCenterManagement;Trusted_Connection=True;TrustServerCertificate=True;`
Bạn cần xác nhận đây là đúng instance SQL Server mà bạn đã xem trong SSMS. Nếu instance trên máy bạn khác, sửa connection string cho khớp.

## Mở màn hình
Từ code-behind của nút/menu dành cho Coach hoặc CenterManager, gọi:
`new AttendanceWindow().ShowDialog();`

Màn hình hiện chưa được gắn tự động vào menu trang chủ vì project gốc chưa có menu chức năng tương ứng.

## Chạy thử
1. Mở file `.sln` của project trong thư mục `SportCenterManament_PRN`.
2. Build Solution.
3. Đăng nhập bằng tài khoản Coach hoặc CenterManager.
4. Mở AttendanceWindow từ nút/menu đã tích hợp.
5. Chọn ngày, chọn buổi học, đánh dấu Có mặt, nhập ghi chú và lưu.

## Phạm vi
- Coach chỉ xem/điểm danh buổi do mình phụ trách.
- CenterManager xem các buổi theo ngày.
- Danh sách học viên lấy từ đăng ký `BOOKED`.
- Mỗi đăng ký có tối đa một bản ghi điểm danh.
