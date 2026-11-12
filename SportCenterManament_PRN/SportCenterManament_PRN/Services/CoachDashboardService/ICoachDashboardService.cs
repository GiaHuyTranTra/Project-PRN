using SportCenterManament_PRN.ViewModels.CoachDashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.Services.CoachDashboardService
{
    public interface ICoachDashboardService
    {
        Task<CoachProfileViewModel?> GetMyProfileAsync();

        Task<List<CoachClassViewModel>> GetMyClassesAsync();

        Task<List<CoachScheduleViewModel>> GetMyScheduleAsync();
    }
}