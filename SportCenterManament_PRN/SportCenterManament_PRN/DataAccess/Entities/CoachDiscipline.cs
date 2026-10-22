using System;
using System.Collections.Generic;

namespace SportCenterManament_PRN.DataAccess.Entities;

public partial class CoachDiscipline
{
    public string CoachAccountId { get; set; } = null!;

    public int DisciplineId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Coach CoachAccount { get; set; } = null!;

    public virtual Discipline Discipline { get; set; } = null!;
}
