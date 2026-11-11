
using System.Collections.Generic;

namespace SportCenterManament_PRN.ViewModels.Coach
{
    public class CoachSaveViewModel
    {
        public string? AccountId { get; set; }
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? Specialization { get; set; }
        public string? WorkSchedule { get; set; }
        public List<int> DisciplineIds { get; set; } = new();
    }
}
