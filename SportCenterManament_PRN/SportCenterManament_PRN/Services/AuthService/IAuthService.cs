using System.Threading.Tasks;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResultViewModel> LoginCenterManagerAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginByRoleAsync(LoginViewModel info, string roleName);
        Task<CheckTokenResultViewModel?> CheckTokenAsync(string tokenValue);
        Task<bool> AuthenticateAsync(string email, string password);
    }
}
