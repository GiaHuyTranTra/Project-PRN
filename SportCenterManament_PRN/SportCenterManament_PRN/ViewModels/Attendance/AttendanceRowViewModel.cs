namespace SportCenterManament_PRN.ViewModels.Attendance;

public class AttendanceRowViewModel
{
    public int ClassRegistrationId { get; set; }
    public string MemberId { get; set; } = string.Empty;
    public string MemberCode { get; set; } = string.Empty;
    public string MemberName { get; set; } = string.Empty;
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
}
