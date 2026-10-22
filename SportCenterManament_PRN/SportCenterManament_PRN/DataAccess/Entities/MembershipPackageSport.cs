using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class MembershipPackageSport
{
    public int PackageId { get; set; }

    public int DisciplineId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual MembershipPackage Package { get; set; } = null!;
}
