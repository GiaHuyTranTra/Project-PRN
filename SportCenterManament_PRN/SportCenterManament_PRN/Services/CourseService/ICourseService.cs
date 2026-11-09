
using SportCenterManament_PRN.ViewModels.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.Services.CourseService
{
    public interface ICourseService
    {
        Task<List<CourseItemViewModel>> GetCoursesAsync(
            string? keyword,
            string? status,
            int? disciplineId);

        Task<List<LookupItemViewModel>> GetDisciplinesAsync();

        Task<List<LookupItemViewModel>> GetCoachesAsync(
            int disciplineId);

        Task<List<LookupItemViewModel>> GetRoomsAsync();

        Task<(bool Success, string Message)> CreateAsync(
            CourseSaveViewModel model);

        Task<(bool Success, string Message)> UpdateAsync(
            CourseSaveViewModel model);

        Task<(bool Success, string Message)> HideAsync(int id);

        Task<(bool Success, string Message)> AssignCoachAsync(
            AssignCoachViewModel model);
    }
}
