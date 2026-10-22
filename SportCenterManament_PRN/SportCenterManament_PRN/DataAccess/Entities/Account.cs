using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class Account
{
    public string Id { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int FailedLoginCount { get; set; }

    public bool IsLocked { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Phone { get; set; }

    public int RoleId { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual CenterManager? CenterManager { get; set; }

    public virtual ICollection<ClassChangeEvent> ClassChangeEvents { get; set; } = new List<ClassChangeEvent>();

    public virtual ICollection<ClassRegistration> ClassRegistrationCanceledByNavigations { get; set; } = new List<ClassRegistration>();

    public virtual ICollection<ClassRegistration> ClassRegistrationRegisteredByNavigations { get; set; } = new List<ClassRegistration>();

    public virtual ICollection<ClassSession> ClassSessionCanceledByNavigations { get; set; } = new List<ClassSession>();

    public virtual ICollection<ClassSession> ClassSessionCreatedByNavigations { get; set; } = new List<ClassSession>();

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual Coach? Coach { get; set; }

    public virtual Member? Member { get; set; }

    public virtual ICollection<MembershipInvoice> MembershipInvoiceCreatedByNavigations { get; set; } = new List<MembershipInvoice>();

    public virtual ICollection<MembershipInvoice> MembershipInvoicePaidByNavigations { get; set; } = new List<MembershipInvoice>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual PasswordChangeOtp? PasswordChangeOtp { get; set; }

    public virtual Receptionist? Receptionist { get; set; }

    public virtual Role Role { get; set; } = null!;
}
