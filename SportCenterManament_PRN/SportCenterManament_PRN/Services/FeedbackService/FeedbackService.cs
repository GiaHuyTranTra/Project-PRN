using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.ViewModels.Feedback;

namespace SportCenterManament_PRN.Services.FeedbackService;

public class FeedbackService : IFeedbackService
{
    public async Task<List<FeedbackSessionOption>> GetEligibleSessionsAsync(
        string memberId)
    {
        await using SportsCenterDbContext context = new SportsCenterDbContext();

        List<FeedbackSessionData> sessionData = await context.ClassAttendances
            .AsNoTracking()
            .Where(attendance =>
                attendance.MemberId == memberId &&
                attendance.IsPresent &&
                !context.Feedbacks.Any(feedback =>
                    feedback.ClassAttendanceId == attendance.Id))
            .OrderByDescending(attendance => attendance.Session.SessionDate)
            .Select(attendance => new FeedbackSessionData
            {
                AttendanceId = attendance.Id,
                ClassName = attendance.Session.Class.Name,
                CoachName = attendance.Session.Coach.FullName,
                SessionDate = attendance.Session.SessionDate
            })
            .ToListAsync();

        List<FeedbackSessionOption> sessions = sessionData
            .Select(session => new FeedbackSessionOption
            {
                AttendanceId = session.AttendanceId,
                Label = $"{session.ClassName} — {session.CoachName} — " +
                    session.SessionDate.ToString("dd/MM/yyyy")
            })
            .ToList();

        return sessions;
    }

    public async Task<List<FeedbackItemViewModel>> GetMemberFeedbackAsync(
        string memberId)
    {
        await using SportsCenterDbContext context = new SportsCenterDbContext();

        List<FeedbackItemViewModel> feedback = await context.Feedbacks
            .AsNoTracking()
            .Where(item => item.ClassAttendance.MemberId == memberId)
            .OrderByDescending(item => item.CreatedAt)
            .Select(item => new FeedbackItemViewModel
            {
                ClassName = item.ClassAttendance.Session.Class.Name,
                CoachName = item.ClassAttendance.Session.Coach.FullName,
                MemberName = item.ClassAttendance.Member.FullName ??
                    item.ClassAttendance.Member.Account.Email,
                Rating = item.Rating,
                Comment = item.Comment,
                CreatedAt = item.CreatedAt
            })
            .ToListAsync();

        return feedback;
    }

    public async Task<(bool Success, string Message)> SubmitFeedbackAsync(
        string memberId,
        int attendanceId,
        int rating,
        string? comment)
    {
        if (rating < 1 || rating > 5)
        {
            return (false, "Số sao phải nằm trong khoảng từ 1 đến 5.");
        }

        if (comment?.Length > 1000)
        {
            return (false, "Nhận xét không được vượt quá 1000 ký tự.");
        }

        try
        {
            await using SportsCenterDbContext context = new SportsCenterDbContext();

            bool hasAttendance = await context.ClassAttendances
                .AnyAsync(attendance =>
                    attendance.Id == attendanceId &&
                    attendance.MemberId == memberId &&
                    attendance.IsPresent);

            if (!hasAttendance)
            {
                return (false,
                    "Chỉ hội viên đã được điểm danh có mặt mới có thể gửi đánh giá.");
            }

            bool alreadySubmitted = await context.Feedbacks
                .AnyAsync(item => item.ClassAttendanceId == attendanceId);

            if (alreadySubmitted)
            {
                return (false, "Buổi học này đã được đánh giá rồi.");
            }

            Feedback feedback = new Feedback
            {
                ClassAttendanceId = attendanceId,
                Rating = rating,
                Comment = string.IsNullOrWhiteSpace(comment)
                    ? null
                    : comment.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await context.Feedbacks.AddAsync(feedback);
            await context.SaveChangesAsync();

            return (true, "Gửi đánh giá thành công.");
        }
        catch (DbUpdateException exception)
        {
            return (false,
                "Không thể lưu đánh giá: " +
                exception.GetBaseException().Message);
        }
        catch (Exception exception)
        {
            return (false,
                "Lỗi khi gửi đánh giá: " +
                exception.GetBaseException().Message);
        }
    }

    public async Task<List<FeedbackItemViewModel>> GetFeedbackAsync(
        string? coachId,
        int? rating)
    {
        await using SportsCenterDbContext context = new SportsCenterDbContext();

        IQueryable<Feedback> query = context.Feedbacks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(coachId))
        {
            query = query.Where(item =>
                item.ClassAttendance.Session.CoachId == coachId);
        }

        if (rating.HasValue)
        {
            query = query.Where(item => item.Rating == rating.Value);
        }

        List<FeedbackItemViewModel> feedback = await query
            .OrderByDescending(item => item.CreatedAt)
            .Select(item => new FeedbackItemViewModel
            {
                ClassName = item.ClassAttendance.Session.Class.Name,
                CoachName = item.ClassAttendance.Session.Coach.FullName,
                MemberName = item.ClassAttendance.Member.FullName ??
                    item.ClassAttendance.Member.Account.Email,
                Rating = item.Rating,
                Comment = item.Comment,
                CreatedAt = item.CreatedAt
            })
            .ToListAsync();

        return feedback;
    }

    public async Task<List<FeedbackAverageViewModel>> GetAveragesAsync(
        string? coachId)
    {
        await using SportsCenterDbContext context = new SportsCenterDbContext();

        IQueryable<Feedback> query = context.Feedbacks.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(coachId))
        {
            query = query.Where(item =>
                item.ClassAttendance.Session.CoachId == coachId);
        }

        List<FeedbackAverageViewModel> averages = await query
            .GroupBy(item => new
            {
                item.ClassAttendance.Session.Class.Name,
                item.ClassAttendance.Session.Coach.FullName
            })
            .Select(group => new FeedbackAverageViewModel
            {
                ClassName = group.Key.Name,
                CoachName = group.Key.FullName,
                FeedbackCount = group.Count(),
                AverageRating = group.Average(item => item.Rating)
            })
            .OrderBy(item => item.ClassName)
            .ThenBy(item => item.CoachName)
            .ToListAsync();

        return averages;
    }

    private sealed class FeedbackSessionData
    {
        public int AttendanceId { get; set; }

        public string ClassName { get; set; } = string.Empty;

        public string CoachName { get; set; } = string.Empty;

        public DateOnly SessionDate { get; set; }
    }
}
