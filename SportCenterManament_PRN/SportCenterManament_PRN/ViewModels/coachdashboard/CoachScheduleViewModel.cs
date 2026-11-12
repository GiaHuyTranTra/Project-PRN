using System;

namespace SportCenterManament_PRN.ViewModels.CoachDashboard
{
    public class CoachScheduleViewModel
    {
        public int SessionId { get; set; }
        public string ClassName { get; set; } = "";
        public string DisciplineName { get; set; } = "";
        public string RoomName { get; set; } = "";
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Capacity { get; set; }
        public int BookedCount { get; set; }
        public string Status { get; set; } = "";
    }
}