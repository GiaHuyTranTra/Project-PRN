namespace SportCenterManament_PRN.ViewModels.Attendance;

public class AttendanceReportViewModel
{
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int TotalMarked { get; set; }
    public int PresentCount { get; set; }
    public int AbsentCount => TotalMarked - PresentCount;
    public double AttendanceRate => TotalMarked == 0 ? 0 : (double)PresentCount * 100 / TotalMarked;
}
