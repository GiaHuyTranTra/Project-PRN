namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class ClassAttendance
{
    public int Id { get; set; }
    public int ClassRegistrationId { get; set; }
    public int SessionId { get; set; }
    public string MemberId { get; set; } = null!;
    public bool IsPresent { get; set; }
    public string? Notes { get; set; }
    public string? MarkedBy { get; set; }
    public DateTime? MarkedAt { get; set; }

    public virtual ClassRegistration ClassRegistration { get; set; } = null!;
    public virtual ClassSession Session { get; set; } = null!;
    public virtual Member Member { get; set; } = null!;
}
