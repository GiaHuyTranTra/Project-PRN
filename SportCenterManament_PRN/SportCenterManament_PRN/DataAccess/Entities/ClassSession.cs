using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class ClassSession
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public string CoachId { get; set; } = null!;

    public int RoomId { get; set; }

    public DateOnly SessionDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int Capacity { get; set; }

    public int BookedCount { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public string? CancelReason { get; set; }

    public DateTime? CanceledAt { get; set; }

    public string? CanceledBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual Account? CanceledByNavigation { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<ClassChangeEvent> ClassChangeEvents { get; set; } = new List<ClassChangeEvent>();

    public virtual ICollection<ClassRegistration> ClassRegistrations { get; set; } = new List<ClassRegistration>();

    public virtual Coach Coach { get; set; } = null!;

    public virtual Account? CreatedByNavigation { get; set; }

    public virtual Room Room { get; set; } = null!;
}
