using System.Threading.Tasks;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Services.AuthService
{
    public interface IAuthService
    {
        Task<LoginResultViewModel> LoginAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginByEmailAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginCenterManagerAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginCoachAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginMemberAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginReceptionistAsync(LoginViewModel info);
        Task<LoginResultViewModel> LoginByRoleAsync(LoginViewModel info, string roleName);
        Task<bool> CheckRoleAsync(string accountId, string expectedRoleName);
        Task<string?> GetAccountRoleAsync(string accountId);
        Task<CheckTokenResultViewModel?> CheckTokenAsync(string tokenValue);
        Task<bool> AuthenticateAsync(string email, string password);
    }
}
