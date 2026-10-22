using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class Discipline
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? NormalizedName { get; set; }

    public byte[] RowVersion { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<CoachDiscipline> CoachDisciplines { get; set; } = new List<CoachDiscipline>();

    public virtual ICollection<MembershipPackageSport> MembershipPackageSports { get; set; } = new List<MembershipPackageSport>();
}
