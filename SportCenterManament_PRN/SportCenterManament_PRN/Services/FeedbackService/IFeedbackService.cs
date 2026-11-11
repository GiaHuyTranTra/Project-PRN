using SportCenterManament_PRN.ViewModels.Feedback;

namespace SportCenterManament_PRN.Services.FeedbackService;

public interface IFeedbackService
{
    Task<List<FeedbackSessionOption>> GetEligibleSessionsAsync(string memberId);

    Task<List<FeedbackItemViewModel>> GetMemberFeedbackAsync(string memberId);

    Task<(bool Success, string Message)> SubmitFeedbackAsync(
        string memberId,
        int attendanceId,
        int rating,
        string? comment);

    Task<List<FeedbackItemViewModel>> GetFeedbackAsync(
        string? coachId,
        int? rating);

    Task<List<FeedbackAverageViewModel>> GetAveragesAsync(string? coachId);
}
