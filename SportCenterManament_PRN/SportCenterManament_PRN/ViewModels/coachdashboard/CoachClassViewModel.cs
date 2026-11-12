namespace SportCenterManament_PRN.ViewModels.CoachDashboard
{
    public class CoachClassViewModel
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public string DisciplineName { get; set; } = "";
        public int Capacity { get; set; }
        public string Status { get; set; } = "";
        public int TotalSessions { get; set; }
    }
}