using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public string RecipientId { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Body { get; set; } = null!;

    public string EntityType { get; set; } = null!;

    public string EntityId { get; set; } = null!;

    public DateTime EventAt { get; set; }

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReadAt { get; set; }

    public Guid? EventId { get; set; }

    public virtual ClassChangeEvent? Event { get; set; }

    public virtual Account Recipient { get; set; } = null!;
}
