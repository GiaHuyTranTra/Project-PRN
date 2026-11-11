
namespace SportCenterManament_PRN.ViewModels.Coach
{
    public class CoachItemViewModel
    {
        public string AccountId { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Phone { get; set; }
        public string? Specialization { get; set; }
        public string? WorkSchedule { get; set; }
        public string Status { get; set; } = "";
        public bool IsLocked { get; set; }
        public string DisciplineNames { get; set; } = "";
    }
}
