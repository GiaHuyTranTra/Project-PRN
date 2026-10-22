using System.Threading.Tasks;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.ViewModels.Account;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Services.AccountService
{
    public interface IAccountService
    {
        Task<bool> IsDuplicateAsync(string email, string phone);
        Task<bool> CreateCenterManagerAsync(AccountViewModel info);
        Task<(bool Success, string Message)> RegisterAccountAsync(RegisterViewModel info);
        Task<Account?> GetAccountByEmailAsync(string email);
        Task<bool> LockAccountAsync(string accountId);
        Task<bool> UnlockAccountAsync(string accountId);
        Task<bool> UnlockAccountByEmailAsync(string email);
    }
}
