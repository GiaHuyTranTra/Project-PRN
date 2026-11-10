namespace SportCenterManament_PRN.ViewModels.Attendance;

public class AttendanceSessionViewModel
{
    public int SessionId { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public DateOnly SessionDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }

    public string DisplayName =>
        $"{ClassName} | {StartTime.ToString("HH:mm")} - {EndTime.ToString("HH:mm")} (#{SessionId})";
}
