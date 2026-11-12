using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.ViewModels.CoachDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.Services.CoachDashboardService
{
    public class CoachDashboardService : ICoachDashboardService
    {
        private string GetCoachId()
        {
            if (!UserSession.IsLoggedIn ||
                !UserSession.IsCoach() ||
                string.IsNullOrWhiteSpace(UserSession.AccountId))
            {
                throw new UnauthorizedAccessException(
                    "Chỉ Coach đã đăng nhập mới được xem thông tin cá nhân.");
            }

            return UserSession.AccountId;
        }

        // THÔNG TIN CÁ NHÂN
        public async Task<CoachProfileViewModel?> GetMyProfileAsync()
        {
            string coachId = GetCoachId();

            using var db = new SportsCenterDbContext();

            return await db.Coaches
                .AsNoTracking()
                .Where(c => c.AccountId == coachId)
                .Select(c => new CoachProfileViewModel
                {
                    FullName = c.FullName,
                    Email = c.Account.Email,
                    Phone = c.Account.Phone ?? "",
                    Specialization = c.Specialization ?? "",
                    WorkSchedule = c.WorkSchedule ?? "",

                    DisciplineNames = string.Join(
                        ", ",
                        c.CoachDisciplines
                            .Select(cd => cd.Discipline.Name))
                })
                .FirstOrDefaultAsync();
        }

        // DANH SÁCH LỚP ĐƯỢC PHÂN CÔNG
        public async Task<List<CoachClassViewModel>> GetMyClassesAsync()
        {
            string coachId = GetCoachId();

            using var db = new SportsCenterDbContext();

            return await db.Classes
                .AsNoTracking()
                .Where(c => c.ClassSessions.Any(
                    s => s.CoachId == coachId &&
                         s.Status != "CANCELED"))
                .Select(c => new CoachClassViewModel
                {
                    ClassId = c.Id,
                    ClassName = c.Name,
                    DisciplineName = c.Discipline.Name,
                    Capacity = c.Capacity,
                    Status = c.Status,

                    TotalSessions = c.ClassSessions.Count(
                        s => s.CoachId == coachId &&
                             s.Status != "CANCELED")
                })
                .OrderBy(c => c.ClassName)
                .ToListAsync();
        }

        // LỊCH DẠY
        public async Task<List<CoachScheduleViewModel>> GetMyScheduleAsync()
        {
            string coachId = GetCoachId();

            using var db = new SportsCenterDbContext();

            return await db.ClassSessions
                .AsNoTracking()
                .Where(s => s.CoachId == coachId)
                .OrderBy(s => s.SessionDate)
                .ThenBy(s => s.StartTime)
                .Select(s => new CoachScheduleViewModel
                {
                    SessionId = s.Id,
                    ClassName = s.Class.Name,
                    DisciplineName = s.Class.Discipline.Name,
                    RoomName = s.Room.Name,
                    SessionDate = s.SessionDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    Capacity = s.Capacity,
                    BookedCount = s.BookedCount,
                    Status = s.Status
                })
                .ToListAsync();
        }
    }
}