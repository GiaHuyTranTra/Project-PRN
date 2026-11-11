
using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.ViewModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.Services.CourseService
{
    public class CourseService : ICourseService
    {
        // TRẠNG THÁI CLASS THEO DATABASE
        private const string ClassDraft = "DRAFT";
        private const string ClassOpen = "OPEN";
        private const string ClassClosed = "CLOSED";
        private const string ClassArchived = "ARCHIVED";

        // TRẠNG THÁI CLASS SESSION
        private const string SessionScheduled = "SCHEDULED";
        private const string SessionCanceled = "CANCELED";

        // Trạng thái tài khoản.
        // Cần khớp với giá trị Account.Status trong database.
        private const string AccountActive = "Active";

        // =====================================================
        // 1. LẤY DANH SÁCH LỚP HỌC
        // =====================================================
        public async Task<List<CourseItemViewModel>> GetCoursesAsync(
            string? keyword,
            string? status,
            int? disciplineId)
        {
            await using var db = new SportsCenterDbContext();

            var query = db.Classes
                .AsNoTracking()
                .AsQueryable();

            // Tìm kiếm theo tên lớp
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim();

                query = query.Where(c =>
                    c.Name.Contains(search));
            }

            // Lọc theo trạng thái
            if (!string.IsNullOrWhiteSpace(status) &&
                status != "All")
            {
                query = query.Where(c =>
                    c.Status == status);
            }

            // Lọc theo bộ môn
            if (disciplineId.HasValue &&
                disciplineId.Value > 0)
            {
                query = query.Where(c =>
                    c.DisciplineId == disciplineId.Value);
            }

            var courses = await query
     .AsNoTracking()
     .Select(c => new
     {
         c.Id,
         c.Name,
         c.Description,
         c.DisciplineId,
         DisciplineName = c.Discipline.Name,
         c.Capacity,
         c.Status,

         CoachNames = c.ClassSessions
             .Where(s => s.Status == "SCHEDULED")
             .Select(s => s.Coach.FullName)
             .Distinct()
             .ToList()
     })
     .ToListAsync();

            return courses.Select(c => new CourseItemViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                DisciplineId = c.DisciplineId,
                DisciplineName = c.DisciplineName,
                Capacity = c.Capacity,
                Status = c.Status,

                CoachNames = c.CoachNames.Count > 0
                    ? string.Join(", ", c.CoachNames)
                    : "Chưa phân công"
            }).ToList();
        }

        // =====================================================
        // 2. LẤY DANH SÁCH BỘ MÔN
        // =====================================================
        public async Task<List<LookupItemViewModel>>
            GetDisciplinesAsync()
        {
            await using var db = new SportsCenterDbContext();

            var disciplines = await db.Disciplines
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new
                {
                    d.Id,
                    d.Name
                })
                .ToListAsync();

            return disciplines
                .Select(d => new LookupItemViewModel
                {
                    Id = d.Id.ToString(),
                    Name = d.Name
                })
                .ToList();
        }

        // =====================================================
        // 3. LẤY DANH SÁCH COACH THEO BỘ MÔN
        // =====================================================
        public async Task<List<LookupItemViewModel>>
            GetCoachesAsync(int disciplineId)
        {
            await using var db = new SportsCenterDbContext();

            return await db.CoachDisciplines
                .AsNoTracking()
                .Where(cd =>
                    cd.DisciplineId == disciplineId &&
                    cd.CoachAccount.Account.Status == AccountActive &&
                    !cd.CoachAccount.Account.IsLocked &&
                    cd.CoachAccount.Account.DeletedAt == null)
                .OrderBy(cd => cd.CoachAccount.FullName)
                .Select(cd => new LookupItemViewModel
                {
                    Id = cd.CoachAccountId,
                    Name = cd.CoachAccount.FullName
                })
                .ToListAsync();
        }

        // =====================================================
        // 4. LẤY DANH SÁCH PHÒNG TẬP
        // =====================================================
        public async Task<List<LookupItemViewModel>>
            GetRoomsAsync()
        {
            await using var db = new SportsCenterDbContext();

            var rooms = await db.Rooms
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .Select(r => new
                {
                    r.Id,
                    r.Name
                })
                .ToListAsync();

            return rooms
                .Select(r => new LookupItemViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name
                })
                .ToList();
        }

        // =====================================================
        // 5. THÊM LỚP HỌC
        // =====================================================
        public async Task<(bool Success, string Message)>
            CreateAsync(CourseSaveViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return (false,
                    "Vui lòng nhập tên lớp học.");
            }

            if (model.Capacity <= 0)
            {
                return (false,
                    "Sức chứa phải lớn hơn 0.");
            }

            if (model.DisciplineId <= 0)
            {
                return (false,
                    "Vui lòng chọn bộ môn.");
            }

            await using var db = new SportsCenterDbContext();

            // Kiểm tra bộ môn có tồn tại và hoạt động không
            bool disciplineExists =
                await db.Disciplines.AnyAsync(d =>
                    d.Id == model.DisciplineId &&
                    d.IsActive);

            if (!disciplineExists)
            {
                return (false,
                    "Bộ môn không tồn tại hoặc đã ngừng hoạt động.");
            }

            string name = model.Name.Trim();

            // Kiểm tra trùng tên lớp chưa lưu trữ
            bool duplicate = await db.Classes.AnyAsync(c =>
                c.Name == name &&
                c.Status != ClassArchived);

            if (duplicate)
            {
                return (false,
                    "Tên lớp học đã tồn tại.");
            }

            var course = new Class
            {
                Name = name,
                Description = model.Description?.Trim(),
                DisciplineId = model.DisciplineId,
                Capacity = model.Capacity,

                // QUAN TRỌNG:
                // Database cho phép OPEN, không cho ACTIVE/PUBLISHED
                Status = ClassOpen,

                CreatedAt = DateTime.UtcNow
            };

            db.Classes.Add(course);

            await db.SaveChangesAsync();

            return (true,
                "Thêm lớp học thành công. Trạng thái: OPEN.");
        }

        // =====================================================
        // 6. CẬP NHẬT LỚP HỌC
        // =====================================================
        public async Task<(bool Success, string Message)>
            UpdateAsync(CourseSaveViewModel model)
        {
            if (!model.Id.HasValue ||
                model.Id.Value <= 0)
            {
                return (false,
                    "Vui lòng chọn lớp học cần sửa.");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return (false,
                    "Vui lòng nhập tên lớp học.");
            }

            if (model.Capacity <= 0)
            {
                return (false,
                    "Sức chứa phải lớn hơn 0.");
            }

            if (model.DisciplineId <= 0)
            {
                return (false,
                    "Vui lòng chọn bộ môn.");
            }

            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .FirstOrDefaultAsync(c =>
                    c.Id == model.Id.Value);

            if (course == null)
            {
                return (false,
                    "Không tìm thấy lớp học.");
            }

            if (course.Status == ClassArchived)
            {
                return (false,
                    "Không thể sửa lớp đã ẩn.");
            }

            if (course.Status == ClassClosed)
            {
                return (false,
                    "Không thể sửa lớp đã đóng.");
            }

            bool disciplineExists =
                await db.Disciplines.AnyAsync(d =>
                    d.Id == model.DisciplineId &&
                    d.IsActive);

            if (!disciplineExists)
            {
                return (false,
                    "Bộ môn không hợp lệ.");
            }

            string name = model.Name.Trim();

            bool duplicate = await db.Classes.AnyAsync(c =>
                c.Id != course.Id &&
                c.Name == name &&
                c.Status != ClassArchived);

            if (duplicate)
            {
                return (false,
                    "Tên lớp học đã tồn tại.");
            }

            // Không được giảm sức chứa nhỏ hơn
            // sức chứa buổi học đã tạo
            bool invalidCapacity =
                await db.ClassSessions.AnyAsync(s =>
                    s.ClassId == course.Id &&
                    s.Status != SessionCanceled &&
                    s.Capacity > model.Capacity);

            if (invalidCapacity)
            {
                return (false,
                    "Sức chứa mới nhỏ hơn sức chứa buổi học hiện có.");
            }

            // Không đổi bộ môn nếu lớp đã có lịch học
            bool hasSessions =
                await db.ClassSessions.AnyAsync(s =>
                    s.ClassId == course.Id &&
                    s.Status != SessionCanceled);

            if (hasSessions &&
                course.DisciplineId != model.DisciplineId)
            {
                return (false,
                    "Không thể đổi bộ môn vì lớp đã có lịch học.");
            }

            course.Name = name;
            course.Description = model.Description?.Trim();
            course.DisciplineId = model.DisciplineId;
            course.Capacity = model.Capacity;
            course.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return (true,
                "Cập nhật lớp học thành công.");
        }

        // =====================================================
        // 7. ẨN LỚP HỌC (SOFT HIDE)
        // =====================================================
        public async Task<(bool Success, string Message)>
            HideAsync(int id)
        {
            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return (false,
                    "Không tìm thấy lớp học.");
            }

            if (course.Status == ClassArchived)
            {
                return (false,
                    "Lớp học đã được ẩn trước đó.");
            }

            DateOnly today =
                DateOnly.FromDateTime(DateTime.Today);

            // Không ẩn nếu vẫn còn lịch chưa hủy trong tương lai
            bool hasFutureSessions =
                await db.ClassSessions.AnyAsync(s =>
                    s.ClassId == id &&
                    s.SessionDate >= today &&
                    s.Status != SessionCanceled);

            if (hasFutureSessions)
            {
                return (false,
                    "Lớp học còn lịch trong tương lai. " +
                    "Vui lòng xử lý lịch trước khi ẩn.");
            }

            course.Status = ClassArchived;
            course.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return (true,
                "Ẩn lớp học thành công.");
        }

        // =====================================================
        // 8. PHÂN CÔNG COACH VÀ TẠO BUỔI HỌC
        // =====================================================
        public async Task<(bool Success, string Message)>
            AssignCoachAsync(AssignCoachViewModel model)
        {
            if (model.ClassId <= 0 ||
                string.IsNullOrWhiteSpace(model.CoachId) ||
                model.RoomId <= 0 ||
                model.Capacity <= 0)
            {
                return (false,
                    "Vui lòng nhập đầy đủ thông tin.");
            }

            if (model.StartTime >= model.EndTime)
            {
                return (false,
                    "Giờ kết thúc phải sau giờ bắt đầu.");
            }

            if (model.SessionDate <
                DateOnly.FromDateTime(DateTime.Today))
            {
                return (false,
                    "Không được tạo lịch trong quá khứ.");
            }

            await using var db = new SportsCenterDbContext();

            // Chỉ cho phép phân công Coach cho lớp OPEN
            var course = await db.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.Id == model.ClassId &&
                    c.Status == ClassOpen);

            if (course == null)
            {
                return (false,
                    "Lớp học không tồn tại hoặc chưa ở trạng thái OPEN.");
            }

            // Kiểm tra Coach thuộc bộ môn của lớp
            bool coachAllowed =
                await db.CoachDisciplines.AnyAsync(cd =>
                    cd.CoachAccountId == model.CoachId &&
                    cd.DisciplineId == course.DisciplineId &&
                    cd.CoachAccount.Account.Status == AccountActive &&
                    !cd.CoachAccount.Account.IsLocked &&
                    cd.CoachAccount.Account.DeletedAt == null);

            if (!coachAllowed)
            {
                return (false,
                    "Coach không đủ điều kiện dạy bộ môn này.");
            }

            // Kiểm tra phòng còn hoạt động
            var room = await db.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.Id == model.RoomId &&
                    r.IsActive);

            if (room == null)
            {
                return (false,
                    "Phòng tập không tồn tại hoặc đã ngừng hoạt động.");
            }

            // Kiểm tra sức chứa lớp và phòng
            if (model.Capacity > course.Capacity)
            {
                return (false,
                    "Sức chứa vượt quá sức chứa của lớp.");
            }

            if (model.Capacity > room.Capacity)
            {
                return (false,
                    "Sức chứa vượt quá sức chứa của phòng.");
            }

            // Kiểm tra các buổi học trùng thời gian
            var overlapping = db.ClassSessions.Where(s =>
                s.SessionDate == model.SessionDate &&
                s.Status != SessionCanceled &&
                model.StartTime < s.EndTime &&
                model.EndTime > s.StartTime);

            // Trùng lịch Coach
            bool coachConflict =
                await overlapping.AnyAsync(s =>
                    s.CoachId == model.CoachId);

            if (coachConflict)
            {
                return (false,
                    "Coach đã có lịch dạy trong khoảng thời gian này.");
            }

            // Trùng lịch phòng
            bool roomConflict =
                await overlapping.AnyAsync(s =>
                    s.RoomId == model.RoomId);

            if (roomConflict)
            {
                return (false,
                    "Phòng tập đã có lịch trong khoảng thời gian này.");
            }

            // Trùng lịch cùng lớp
            bool classConflict =
                await overlapping.AnyAsync(s =>
                    s.ClassId == model.ClassId);

            if (classConflict)
            {
                return (false,
                    "Lớp học đã có buổi học trùng thời gian.");
            }

            var session = new ClassSession
            {
                ClassId = model.ClassId,
                CoachId = model.CoachId,
                RoomId = model.RoomId,
                SessionDate = model.SessionDate,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                Capacity = model.Capacity,
                BookedCount = 0,

                Status = SessionScheduled,

                Notes = model.Notes?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            db.ClassSessions.Add(session);

            await db.SaveChangesAsync();

            return (true,
                "Phân công Coach và tạo buổi học thành công.");
        }




        // 9. KHÔI PHỤC LỚP ĐÃ ẨN
        public async Task<(bool Success, string Message)>
            RestoreAsync(int id)
        {
            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return (false, "Không tìm thấy lớp học.");
            }

            // Chỉ khôi phục lớp đã ẩn
            if (course.Status != "ARCHIVED")
            {
                return (false, "Lớp học này chưa bị ẩn.");
            }

            // Kiểm tra tên trùng với lớp chưa ẩn
            bool duplicate = await db.Classes.AnyAsync(c =>
                c.Id != id &&
                c.Name == course.Name &&
                c.Status != "ARCHIVED");

            if (duplicate)
            {
                return (false,
                    "Không thể khôi phục vì đã có lớp khác cùng tên.");
            }

            // Khôi phục về trạng thái hoạt động
            course.Status = "OPEN";
            course.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return (true, "Khôi phục lớp học thành công.");
        }

    
    
// 10. XÓA CỨNG LỚP HỌC
public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return (false, "Không tìm thấy lớp học.");
            }

            // Kiểm tra lớp đã có buổi học hay chưa
            bool hasSessions = await db.ClassSessions
                .AnyAsync(s => s.ClassId == id);

            if (hasSessions)
            {
                return (false,
                    "Không thể xóa cứng vì lớp đã có buổi học. " +
                    "Bạn có thể dùng chức năng Ẩn lớp.");
            }

            // XÓA CỨNG KHỎI DATABASE
            db.Classes.Remove(course);

            try
            {
                await db.SaveChangesAsync();

                return (true, "Xóa lớp học thành công.");
            }
            catch (DbUpdateException ex)
            {
                return (false,
                    "Không thể xóa lớp vì dữ liệu đang được sử dụng " +
                    "ở bảng khác. Chi tiết: " +
                    ex.GetBaseException().Message);
            }
        }


    }}
