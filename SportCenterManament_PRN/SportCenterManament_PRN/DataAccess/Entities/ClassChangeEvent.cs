using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class ClassChangeEvent
{
    public Guid Id { get; set; }

    public Guid OperationId { get; set; }

    public int SessionId { get; set; }

    public string EventType { get; set; } = null!;

    public string? ActorAccountId { get; set; }

    public DateTime OccurredAt { get; set; }

    public virtual Account? ActorAccount { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ClassSession Session { get; set; } = null!;
}
