
using System;

namespace SportCenterManament_PRN.ViewModels.Course
{
    public class CourseItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisciplineId { get; set; }
        public string DisciplineName { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Status { get; set; } = "Active";

        public string CoachNames { get; set; } = "";
    }

    public class CourseSaveViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisciplineId { get; set; }
        public int Capacity { get; set; }
    }

    public class AssignCoachViewModel
    {
        public int ClassId { get; set; }
        public string CoachId { get; set; } = string.Empty;
        public int RoomId { get; set; }
        public DateOnly SessionDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Capacity { get; set; }
        public string? Notes { get; set; }
    }

    public class LookupItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
