namespace SportCenterManament_PRN.ViewModels.Feedback;

public class FeedbackAverageViewModel
{
    public string ClassName { get; set; } = string.Empty;

    public string CoachName { get; set; } = string.Empty;

    public int FeedbackCount { get; set; }

    public double AverageRating { get; set; }
}
