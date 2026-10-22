using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class ClassRegistration
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public string MemberId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime RegisteredAt { get; set; }

    public string RegisteredVia { get; set; } = null!;

    public string RegisteredBy { get; set; } = null!;

    public DateTime? CanceledAt { get; set; }

    public string? CanceledBy { get; set; }

    public string? CancelReason { get; set; }

    public int? MemberSubscriptionId { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual Account? CanceledByNavigation { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual MemberSubscription? MemberSubscription { get; set; }

    public virtual Account RegisteredByNavigation { get; set; } = null!;

    public virtual ClassSession Session { get; set; } = null!;
}
