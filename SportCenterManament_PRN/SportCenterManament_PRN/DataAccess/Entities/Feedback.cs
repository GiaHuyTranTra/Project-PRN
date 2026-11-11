namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class Feedback
{
    public int Id { get; set; }

    public int ClassAttendanceId { get; set; }

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ClassAttendance ClassAttendance { get; set; } = null!;
}
