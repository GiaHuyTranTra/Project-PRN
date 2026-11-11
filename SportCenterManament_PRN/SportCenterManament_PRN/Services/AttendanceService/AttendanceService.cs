using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.ViewModels.Attendance;

namespace SportCenterManament_PRN.Services.AttendanceService;

public class AttendanceService
{
    public async Task<List<AttendanceSessionViewModel>> GetSessionsAsync(DateOnly sessionDate)
    {
        await using SportsCenterDbContext context = new SportsCenterDbContext();

        IQueryable<ClassSession> query = context.ClassSessions
            .AsNoTracking()
            .Include(session => session.Class)
            .Where(session => session.SessionDate == sessionDate &&
                              session.Status == "SCHEDULED");

        if (UserSession.IsCoach())
        {
            query = query.Where(session => session.CoachId == UserSession.AccountId);
        }
        else if (!UserSession.IsCenterManager())
        {
            throw new UnauthorizedAccessException("Bạn không có quyền xem lịch điểm danh.");
        }

        return await query.OrderBy(session => session.StartTime)
            .Select(session => new AttendanceSessionViewModel
            {
                SessionId = session.Id,
                ClassId = session.ClassId,
                ClassName = session.Class.Name,
                SessionDate = session.SessionDate,
                StartTime = session.StartTime,
                EndTime = session.EndTime
            })
            .ToListAsync();
    }

    public async Task<List<AttendanceRowViewModel>> GetAttendanceRowsAsync(int sessionId)
    {
        await using SportsCenterDbContext context = new SportsCenterDbContext();

        ClassSession? session = await context.ClassSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == sessionId);

        if (session == null)
        {
            throw new InvalidOperationException("Không tìm thấy buổi học.");
        }

        EnsureCanAccessSession(session);

        return await context.ClassRegistrations
            .AsNoTracking()
            .Where(registration => registration.SessionId == sessionId &&
                                   registration.Status == "BOOKED")
            .OrderBy(registration => registration.Member.FullName)
            .Select(registration => new AttendanceRowViewModel
            {
                ClassRegistrationId = registration.Id,
                MemberId = registration.MemberId,
                MemberCode = registration.Member.MemberCode,
                MemberName = registration.Member.FullName ?? registration.Member.MemberCode,
                IsPresent = registration.ClassAttendance != null &&
                            registration.ClassAttendance.IsPresent,
                Notes = registration.ClassAttendance == null
                    ? null
                    : registration.ClassAttendance.Notes
            })
            .ToListAsync();
    }

    public async Task SaveAttendanceAsync(
        int sessionId,
        IReadOnlyCollection<AttendanceRowViewModel> rows)
    {
        if (!UserSession.IsCoach() && !UserSession.IsCenterManager())
        {
            throw new UnauthorizedAccessException("Bạn không có quyền lưu điểm danh.");
        }

        await using SportsCenterDbContext context = new SportsCenterDbContext();

        ClassSession? session = await context.ClassSessions
            .FirstOrDefaultAsync(item => item.Id == sessionId);

        if (session == null)
        {
            throw new InvalidOperationException("Không tìm thấy buổi học.");
        }

        EnsureCanAccessSession(session);

        List<int> registrationIds = rows
            .Select(row => row.ClassRegistrationId)
            .Distinct()
            .ToList();

        if (registrationIds.Count != rows.Count)
        {
            throw new InvalidOperationException("Danh sách điểm danh có mã đăng ký trùng lặp.");
        }

        List<ClassRegistration> registrations = await context.ClassRegistrations
            .Where(registration => registrationIds.Contains(registration.Id) &&
                                   registration.SessionId == sessionId &&
                                   registration.Status == "BOOKED")
            .ToListAsync();

        if (registrations.Count != registrationIds.Count)
        {
            throw new InvalidOperationException(
                "Danh sách học viên không hợp lệ hoặc đã thay đổi. Vui lòng tải lại.");
        }

        Dictionary<int, ClassRegistration> registrationById =
            registrations.ToDictionary(registration => registration.Id);

        List<ClassAttendance> existing = await context.ClassAttendances
            .Where(attendance => attendance.SessionId == sessionId &&
                                 registrationIds.Contains(attendance.ClassRegistrationId))
            .ToListAsync();

        Dictionary<int, ClassAttendance> existingByRegistration = existing
            .ToDictionary(attendance => attendance.ClassRegistrationId);

        List<ClassAttendance> newAttendances = new List<ClassAttendance>();

        foreach (AttendanceRowViewModel row in rows)
        {
            ClassRegistration registration = registrationById[row.ClassRegistrationId];

            if (!string.Equals(registration.MemberId, row.MemberId, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Thông tin học viên không khớp đăng ký lớp.");
            }

            if (existingByRegistration.TryGetValue(
                    row.ClassRegistrationId, out ClassAttendance? attendance))
            {
                attendance.IsPresent = row.IsPresent;
                attendance.Notes = NormalizeNotes(row.Notes);
                attendance.MarkedBy = UserSession.AccountId;
                attendance.MarkedAt = DateTime.UtcNow;
            }
            else
            {
                newAttendances.Add(new ClassAttendance
                {
                    ClassRegistrationId = row.ClassRegistrationId,
                    SessionId = sessionId,
                    MemberId = row.MemberId,
                    IsPresent = row.IsPresent,
                    Notes = NormalizeNotes(row.Notes),
                    MarkedBy = UserSession.AccountId,
                    MarkedAt = DateTime.UtcNow
                });
            }
        }

        if (newAttendances.Count > 0)
        {
            await context.ClassAttendances.AddRangeAsync(newAttendances);
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<AttendanceReportViewModel>> GetAttendanceReportAsync()
    {
        if (!UserSession.IsCenterManager() && !UserSession.IsCoach())
        {
            throw new UnauthorizedAccessException("Bạn không có quyền xem báo cáo điểm danh.");
        }

        await using SportsCenterDbContext context = new SportsCenterDbContext();

        IQueryable<ClassAttendance> query = context.ClassAttendances.AsNoTracking();

        if (UserSession.IsCoach())
        {
            query = query.Where(attendance =>
                attendance.Session.CoachId == UserSession.AccountId);
        }

        return await query
            .GroupBy(attendance => new
            {
                attendance.Session.ClassId,
                attendance.Session.Class.Name
            })
            .Select(group => new AttendanceReportViewModel
            {
                ClassId = group.Key.ClassId,
                ClassName = group.Key.Name,
                TotalMarked = group.Count(),
                PresentCount = group.Count(attendance => attendance.IsPresent)
            })
            .OrderBy(report => report.ClassName)
            .ToListAsync();
    }

    private static void EnsureCanAccessSession(ClassSession session)
    {
        if (UserSession.IsCoach() &&
            !string.Equals(session.CoachId, UserSession.AccountId, StringComparison.Ordinal))
        {
            throw new UnauthorizedAccessException(
                "Bạn chỉ được điểm danh buổi học do mình phụ trách.");
        }

        if (!UserSession.IsCoach() && !UserSession.IsCenterManager())
        {
            throw new UnauthorizedAccessException("Bạn không có quyền điểm danh.");
        }
    }

    private static string? NormalizeNotes(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes))
        {
            return null;
        }

        string normalized = notes.Trim();
        if (normalized.Length > 500)
        {
            throw new ArgumentException("Ghi chú không được vượt quá 500 ký tự.");
        }

        return normalized;
    }
}
