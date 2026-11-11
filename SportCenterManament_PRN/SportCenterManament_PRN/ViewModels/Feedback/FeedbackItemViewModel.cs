namespace SportCenterManament_PRN.ViewModels.Feedback;

public class FeedbackItemViewModel
{
    public string ClassName { get; set; } = string.Empty;

    public string CoachName { get; set; } = string.Empty;

    public string MemberName { get; set; } = string.Empty;

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }
}
