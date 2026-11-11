
using SportCenterManament_PRN.ViewModels.Coach;
using SportCenterManament_PRN.ViewModels.Course;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.Services.CoachService
{
    public interface ICoachService
    {
        Task<List<CoachItemViewModel>> GetAllAsync(string? keyword);

        Task<List<LookupItemViewModel>> GetDisciplinesAsync();

        Task<List<int>> GetCoachDisciplineIdsAsync(string accountId);

        Task<(bool Success, string Message)> CreateAsync(
            CoachSaveViewModel model);

        Task<(bool Success, string Message)> UpdateAsync(
            CoachSaveViewModel model);

        Task<(bool Success, string Message)> SetLockedAsync(
            string accountId, bool locked);
    }
}
