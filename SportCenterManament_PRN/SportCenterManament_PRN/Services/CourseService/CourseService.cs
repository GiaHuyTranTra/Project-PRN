
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
        public async Task<List<CourseItemViewModel>> GetCoursesAsync(
            string? keyword, string? status, int? disciplineId)
        {
            await using var db = new SportsCenterDbContext();

            var query = db.Classes.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string term = keyword.Trim();
                query = query.Where(c => c.Name.Contains(term));
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                query = query.Where(c => c.Status == status);
            }

            if (disciplineId.HasValue && disciplineId.Value > 0)
            {
                query = query.Where(c =>
                    c.DisciplineId == disciplineId.Value);
            }

            return await query
                .OrderBy(c => c.Name)
                .Select(c => new CourseItemViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    DisciplineId = c.DisciplineId,
                    DisciplineName = c.Discipline.Name,
                    Capacity = c.Capacity,
                    Status = c.Status
                })
                .ToListAsync();
        }

        public async Task<List<LookupItemViewModel>> GetDisciplinesAsync()
        {
            await using var db = new SportsCenterDbContext();

            return await db.Disciplines
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new LookupItemViewModel
                {
                    Id = d.Id.ToString(),
                    Name = d.Name
                })
                .ToListAsync();
        }

        public async Task<List<LookupItemViewModel>> GetCoachesAsync(
            int disciplineId)
        {
            await using var db = new SportsCenterDbContext();

            return await db.CoachDisciplines
                .AsNoTracking()
                .Where(cd =>
                    cd.DisciplineId == disciplineId &&
                    cd.CoachAccount.Account.Status == "Active" &&
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

        public async Task<List<LookupItemViewModel>> GetRoomsAsync()
        {
            await using var db = new SportsCenterDbContext();

            return await db.Rooms
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.Name)
                .Select(r => new LookupItemViewModel
                {
                    Id = r.Id.ToString(),
                    Name = r.Name
                })
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)> CreateAsync(
            CourseSaveViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) ||
                model.Capacity <= 0 ||
                model.DisciplineId <= 0)
            {
                return (false,
                    "Vui lòng nhập tên lớp, bộ môn và sức chứa hợp lệ.");
            }

            await using var db = new SportsCenterDbContext();

            bool disciplineExists = await db.Disciplines.AnyAsync(
                d => d.Id == model.DisciplineId && d.IsActive);

            if (!disciplineExists)
                return (false, "Bộ môn không hợp lệ.");

            string name = model.Name.Trim();

            bool duplicate = await db.Classes.AnyAsync(c =>
                c.Name == name &&
                c.DisciplineId == model.DisciplineId &&
                c.Status != "Inactive");

            if (duplicate)
                return (false, "Tên lớp đã tồn tại trong bộ môn này.");

            var course = new Class
            {
                Name = name,
                Description = model.Description?.Trim(),
                DisciplineId = model.DisciplineId,
                Capacity = model.Capacity,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            db.Classes.Add(course);
            await db.SaveChangesAsync();

            return (true, "Thêm lớp học thành công.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            CourseSaveViewModel model)
        {
            if (!model.Id.HasValue ||
                string.IsNullOrWhiteSpace(model.Name) ||
                model.Capacity <= 0 ||
                model.DisciplineId <= 0)
            {
                return (false, "Thông tin lớp học không hợp lệ.");
            }

            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .FirstOrDefaultAsync(c => c.Id == model.Id.Value);

            if (course == null)
                return (false, "Không tìm thấy lớp học.");

            if (course.Status == "Inactive")
                return (false, "Không thể sửa lớp đã ẩn.");

            bool disciplineExists = await db.Disciplines.AnyAsync(
                d => d.Id == model.DisciplineId && d.IsActive);

            if (!disciplineExists)
                return (false, "Bộ môn không hợp lệ.");

            string name = model.Name.Trim();

            bool duplicate = await db.Classes.AnyAsync(c =>
                c.Id != course.Id &&
                c.Name == name &&
                c.DisciplineId == model.DisciplineId &&
                c.Status != "Inactive");

            if (duplicate)
                return (false, "Tên lớp đã tồn tại trong bộ môn này.");

            bool invalidCapacity = await db.ClassSessions.AnyAsync(s =>
                s.ClassId == course.Id &&
                s.Status != "Canceled" &&
                s.Capacity > model.Capacity);

            if (invalidCapacity)
                return (false,
                    "Sức chứa mới nhỏ hơn sức chứa của buổi học hiện có.");

            course.Name = name;
            course.Description = model.Description?.Trim();
            course.DisciplineId = model.DisciplineId;
            course.Capacity = model.Capacity;
            course.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return (true, "Cập nhật lớp học thành công.");
        }

        public async Task<(bool Success, string Message)> HideAsync(int id)
        {
            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return (false, "Không tìm thấy lớp học.");

            if (course.Status == "Inactive")
                return (false, "Lớp học đã được ẩn.");

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            bool hasFutureSessions = await db.ClassSessions.AnyAsync(s =>
                s.ClassId == id &&
                s.SessionDate >= today &&
                s.Status != "Canceled");

            if (hasFutureSessions)
            {
                return (false,
                    "Lớp còn lịch học trong tương lai. " +
                    "Vui lòng xử lý lịch trước khi ẩn.");
            }

            course.Status = "Inactive";
            course.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return (true, "Ẩn lớp học thành công.");
        }

        public async Task<(bool Success, string Message)> AssignCoachAsync(
            AssignCoachViewModel model)
        {
            if (model.ClassId <= 0 ||
                string.IsNullOrWhiteSpace(model.CoachId) ||
                model.RoomId <= 0 ||
                model.Capacity <= 0)
            {
                return (false, "Vui lòng nhập đầy đủ thông tin.");
            }

            if (model.StartTime >= model.EndTime)
                return (false, "Giờ kết thúc phải sau giờ bắt đầu.");

            if (model.SessionDate < DateOnly.FromDateTime(DateTime.Today))
                return (false, "Không được tạo lịch trong quá khứ.");

            await using var db = new SportsCenterDbContext();

            var course = await db.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c =>
                    c.Id == model.ClassId &&
                    c.Status == "Active");

            if (course == null)
                return (false, "Lớp học không tồn tại hoặc đã ẩn.");

            bool coachAllowed = await db.CoachDisciplines.AnyAsync(cd =>
                cd.CoachAccountId == model.CoachId &&
                cd.DisciplineId == course.DisciplineId &&
                cd.CoachAccount.Account.Status == "Active" &&
                !cd.CoachAccount.Account.IsLocked &&
                cd.CoachAccount.Account.DeletedAt == null);

            if (!coachAllowed)
                return (false,
                    "Huấn luyện viên không đủ điều kiện dạy bộ môn này.");

            var room = await db.Rooms
                .AsNoTracking()
                .FirstOrDefaultAsync(r =>
                    r.Id == model.RoomId && r.IsActive);

            if (room == null)
                return (false, "Phòng tập không hợp lệ.");

            if (model.Capacity > course.Capacity ||
                model.Capacity > room.Capacity)
            {
                return (false,
                    "Sức chứa vượt quá giới hạn của lớp hoặc phòng.");
            }

            var overlapping = db.ClassSessions.Where(s =>
                s.SessionDate == model.SessionDate &&
                s.Status != "Canceled" &&
                model.StartTime < s.EndTime &&
                model.EndTime > s.StartTime);

            bool coachConflict = await overlapping.AnyAsync(s =>
                s.CoachId == model.CoachId);

            if (coachConflict)
                return (false, "Huấn luyện viên bị trùng lịch.");

            bool roomConflict = await overlapping.AnyAsync(s =>
                s.RoomId == model.RoomId);

            if (roomConflict)
                return (false, "Phòng tập bị trùng lịch.");

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
                Status = "Scheduled",
                Notes = model.Notes?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            db.ClassSessions.Add(session);
            await db.SaveChangesAsync();

            return (true,
                "Phân công Coach và tạo buổi học thành công.");
        }
    }
}
