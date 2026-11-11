
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.ViewModels.Coach;
using SportCenterManament_PRN.ViewModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.Services.CoachService
{
    public class CoachService : ICoachService
    {
        private const string ActiveStatus = "Active";

        private static string HashPassword(string password)
        {
            // PHẢI đồng bộ với cách kiểm tra mật khẩu
            // trong AuthService của project.
            var hasher = new PasswordHasher<Account>();

            return hasher.HashPassword(
                new Account(), password);
        }

        public async Task<List<CoachItemViewModel>>
            GetAllAsync(string? keyword)
        {
            await using var db = new SportsCenterDbContext();

            var query = db.Coaches
                .AsNoTracking()
                .Where(c => c.Account.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                string search = keyword.Trim();

                query = query.Where(c =>
                    c.FullName.Contains(search) ||
                    c.Account.Email.Contains(search));
            }

            var coaches = await query
                .OrderBy(c => c.FullName)
                .Select(c => new
                {
                    c.AccountId,
                    c.FullName,
                    c.Specialization,
                    c.WorkSchedule,
                    c.Account.Email,
                    c.Account.Phone,
                    c.Account.Status,
                    c.Account.IsLocked,
                    Disciplines = c.CoachDisciplines
                        .Select(cd => cd.Discipline.Name)
                        .ToList()
                })
                .ToListAsync();

            return coaches.Select(c => new CoachItemViewModel
            {
                AccountId = c.AccountId,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                Specialization = c.Specialization,
                WorkSchedule = c.WorkSchedule,
                Status = c.Status,
                IsLocked = c.IsLocked,
                DisciplineNames = string.Join(", ", c.Disciplines)
            }).ToList();
        }

        public async Task<List<LookupItemViewModel>>
            GetDisciplinesAsync()
        {
            await using var db = new SportsCenterDbContext();

            var disciplines = await db.Disciplines
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return disciplines.Select(d => new LookupItemViewModel
            {
                Id = d.Id.ToString(),
                Name = d.Name
            }).ToList();
        }

        public async Task<List<int>>
            GetCoachDisciplineIdsAsync(string accountId)
        {
            await using var db = new SportsCenterDbContext();

            return await db.CoachDisciplines
                .AsNoTracking()
                .Where(cd => cd.CoachAccountId == accountId)
                .Select(cd => cd.DisciplineId)
                .ToListAsync();
        }

        public async Task<(bool Success, string Message)>
            CreateAsync(CoachSaveViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FullName) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return (false,
                    "Vui lòng nhập họ tên, email và mật khẩu.");
            }

            if (model.Password.Length < 6)
                return (false, "Mật khẩu phải có ít nhất 6 ký tự.");

            if (model.DisciplineIds.Count == 0)
                return (false, "Vui lòng chọn ít nhất một bộ môn.");

            await using var db = new SportsCenterDbContext();

            string email = model.Email.Trim();

            bool emailExists = await db.Accounts
                .AnyAsync(a => a.Email == email);

            if (emailExists)
                return (false, "Email đã tồn tại.");

            // Đối chiếu tên Role với dữ liệu thật.
            var coachRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Name == "Coach");

            if (coachRole == null)
                return (false, "Không tìm thấy Role Coach.");

            var disciplineIds = model.DisciplineIds.Distinct().ToList();

            int validCount = await db.Disciplines
                .CountAsync(d =>
                    disciplineIds.Contains(d.Id) && d.IsActive);

            if (validCount != disciplineIds.Count)
                return (false, "Danh sách bộ môn không hợp lệ.");

            string accountId = Guid.NewGuid().ToString();

            var account = new Account
            {
                Id = accountId,
                Email = email,
                Phone = model.Phone?.Trim(),
                PasswordHash = HashPassword(model.Password),
                RoleId = coachRole.Id,
                Status = ActiveStatus,
                IsLocked = false,
                FailedLoginCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            var coach = new SportCenterManament_PRN.DataAccess.Entities.Coach
            {
                AccountId = accountId,
                FullName = model.FullName.Trim(),
                Specialization = model.Specialization?.Trim(),
                WorkSchedule = model.WorkSchedule?.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await using var transaction =
                await db.Database.BeginTransactionAsync();

            try
            {
                db.Accounts.Add(account);
                await db.SaveChangesAsync();

                db.Coaches.Add(coach);
                await db.SaveChangesAsync();

                foreach (int disciplineId in disciplineIds)
                {
                    db.CoachDisciplines.Add(new CoachDiscipline
                    {
                        CoachAccountId = accountId,
                        DisciplineId = disciplineId,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Thêm Coach thành công.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return (false,
                    "Lỗi thêm Coach: " +
                    ex.GetBaseException().Message);
            }
        }

        public async Task<(bool Success, string Message)>
            UpdateAsync(CoachSaveViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.AccountId) ||
                string.IsNullOrWhiteSpace(model.FullName) ||
                string.IsNullOrWhiteSpace(model.Email))
            {
                return (false, "Thông tin Coach không hợp lệ.");
            }

            if (model.DisciplineIds.Count == 0)
                return (false, "Vui lòng chọn ít nhất một bộ môn.");

            await using var db = new SportsCenterDbContext();

            var coach = await db.Coaches
                .Include(c => c.Account)
                .Include(c => c.CoachDisciplines)
                .FirstOrDefaultAsync(c =>
                    c.AccountId == model.AccountId &&
                    c.Account.DeletedAt == null);

            if (coach == null)
                return (false, "Không tìm thấy Coach.");

            string email = model.Email.Trim();

            bool duplicate = await db.Accounts.AnyAsync(a =>
                a.Id != coach.AccountId && a.Email == email);

            if (duplicate)
                return (false, "Email đã được sử dụng.");

            var ids = model.DisciplineIds.Distinct().ToList();

            int validCount = await db.Disciplines
                .CountAsync(d => ids.Contains(d.Id) && d.IsActive);

            if (validCount != ids.Count)
                return (false, "Bộ môn không hợp lệ.");

            // Không bỏ chuyên môn đang được sử dụng
            // bởi các buổi học chưa hủy.
            var assignedDisciplineIds = await db.ClassSessions
                .Where(s =>
                    s.CoachId == coach.AccountId &&
                    s.Status != "CANCELED")
                .Select(s => s.Class.DisciplineId)
                .Distinct()
                .ToListAsync();

            if (assignedDisciplineIds.Any(id => !ids.Contains(id)))
            {
                return (false,
                    "Không thể bỏ bộ môn vì Coach đã được phân công " +
                    "vào lớp thuộc bộ môn đó.");
            }

            coach.FullName = model.FullName.Trim();
            coach.Specialization = model.Specialization?.Trim();
            coach.WorkSchedule = model.WorkSchedule?.Trim();

            coach.Account.Email = email;
            coach.Account.Phone = model.Phone?.Trim();
            coach.Account.UpdatedAt = DateTime.UtcNow;

            await using var transaction =
                await db.Database.BeginTransactionAsync();

            try
            {
                db.CoachDisciplines.RemoveRange(
                    coach.CoachDisciplines);

                foreach (int id in ids)
                {
                    db.CoachDisciplines.Add(new CoachDiscipline
                    {
                        CoachAccountId = coach.AccountId,
                        DisciplineId = id,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                await db.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Cập nhật Coach thành công.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return (false,
                    "Lỗi cập nhật: " +
                    ex.GetBaseException().Message);
            }
        }

        public async Task<(bool Success, string Message)>
            SetLockedAsync(string accountId, bool locked)
        {
            await using var db = new SportsCenterDbContext();

            var coach = await db.Coaches
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c =>
                    c.AccountId == accountId &&
                    c.Account.DeletedAt == null);

            if (coach == null)
                return (false, "Không tìm thấy Coach.");

            if (locked)
            {
                bool hasFutureSessions = await db.ClassSessions
                    .AnyAsync(s =>
                        s.CoachId == accountId &&
                        s.Status != "CANCELED" &&
                        s.SessionDate >=
                            DateOnly.FromDateTime(DateTime.Today));

                if (hasFutureSessions)
                {
                    return (false,
                        "Coach còn lịch dạy trong tương lai. " +
                        "Vui lòng xử lý lịch trước khi khóa.");
                }
            }

            coach.Account.IsLocked = locked;
            coach.Account.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return (true, locked
                ? "Khóa Coach thành công."
                : "Mở khóa Coach thành công.");
        }
    }
}
