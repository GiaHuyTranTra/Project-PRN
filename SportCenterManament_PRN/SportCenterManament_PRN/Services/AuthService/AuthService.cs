using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.Services.AccountService;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Services.AuthService
{
    public class AuthService : IAuthService
    {
        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            LoginViewModel info = new LoginViewModel
            {
                Email = email,
                Password = password
            };

            LoginResultViewModel result = await LoginAsync(info);
            return result.Success;
        }

        public async Task<LoginResultViewModel> LoginAsync(LoginViewModel info)
        {
            return await LoginCoreAsync(info, null);
        }

        public async Task<LoginResultViewModel> LoginByEmailAsync(LoginViewModel info)
        {
            return await LoginCoreAsync(info, null);
        }

        public async Task<LoginResultViewModel> LoginCenterManagerAsync(LoginViewModel info)
        {
            return await LoginByRoleAsync(info, "CenterManager");
        }

        public async Task<LoginResultViewModel> LoginCoachAsync(LoginViewModel info)
        {
            return await LoginByRoleAsync(info, "Coach");
        }

        public async Task<LoginResultViewModel> LoginMemberAsync(LoginViewModel info)
        {
            return await LoginByRoleAsync(info, "Member");
        }

        public async Task<LoginResultViewModel> LoginReceptionistAsync(LoginViewModel info)
        {
            return await LoginByRoleAsync(info, "Receptionist");
        }

        public async Task<LoginResultViewModel> LoginByRoleAsync(LoginViewModel info, string roleName)
        {
            return await LoginCoreAsync(info, roleName);
        }

        public async Task<bool> CheckRoleAsync(string accountId, string expectedRoleName)
        {
            if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(expectedRoleName))
            {
                return false;
            }

            try
            {
                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Account? account = await context.Accounts
                        .AsNoTracking()
                        .Include(a => a.Role)
                        .FirstOrDefaultAsync(a => a.Id == accountId);

                    if (account == null || account.Role == null)
                    {
                        return false;
                    }

                    return string.Equals(account.Role.Name, expectedRoleName, StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
            }
        }

        public async Task<string?> GetAccountRoleAsync(string accountId)
        {
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return null;
            }

            try
            {
                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Account? account = await context.Accounts
                        .AsNoTracking()
                        .Include(a => a.Role)
                        .FirstOrDefaultAsync(a => a.Id == accountId);

                    return account?.Role?.Name;
                }
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
            }
        }

        private async Task<LoginResultViewModel> LoginCoreAsync(LoginViewModel info, string? requiredRoleName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(info.Email) || string.IsNullOrWhiteSpace(info.Password))
                {
                    return new LoginResultViewModel
                    {
                        Success = false,
                        Message = "Vui lòng nhập đầy đủ Email và Mật khẩu."
                    };
                }

                string normalizedEmail = info.Email.Trim().ToLower();

                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Account? account = await context.Accounts
                        .Include(q => q.Role)
                        .Include(q => q.CenterManager)
                        .Include(q => q.Coach)
                        .Include(q => q.Member)
                        .FirstOrDefaultAsync(q => q.Email.ToLower() == normalizedEmail);

                    if (account == null)
                    {
                        return new LoginResultViewModel
                        {
                            Success = false,
                            Message = "Email hoặc mật khẩu không chính xác."
                        };
                    }

                    if (requiredRoleName != null &&
                        (account.Role == null || !string.Equals(account.Role.Name, requiredRoleName, StringComparison.OrdinalIgnoreCase)))
                    {
                        return new LoginResultViewModel
                        {
                            Success = false,
                            Message = $"Quyền truy cập bị từ chối. Vai trò yêu cầu là {requiredRoleName}."
                        };
                    }

                    if (account.DeletedAt != null)
                    {
                        return new LoginResultViewModel
                        {
                            Success = false,
                            Message = "Tài khoản này đã bị xóa khỏi hệ thống."
                        };
                    }

                    if (account.IsLocked)
                    {
                        return new LoginResultViewModel
                        {
                            Success = false,
                            Message = "Tài khoản của bạn đã bị khóa. Vui lòng liên hệ quản trị viên."
                        };
                    }

                    if (!string.Equals(account.Status, "Active", StringComparison.OrdinalIgnoreCase))
                    {
                        return new LoginResultViewModel
                        {
                            Success = false,
                            Message = "Tài khoản hiện đang ở trạng thái ngừng hoạt động."
                        };
                    }

                    bool isPasswordValid = HashService.VerifyPassword(info.Password, account.PasswordHash);

                    if (!isPasswordValid)
                    {
                        account.FailedLoginCount++;
                        if (account.FailedLoginCount >= 5)
                        {
                            account.IsLocked = true;
                        }
                        account.UpdatedAt = DateTime.UtcNow;
                        await context.SaveChangesAsync();

                        if (account.IsLocked)
                        {
                            return new LoginResultViewModel
                            {
                                Success = false,
                                Message = "Mật khẩu sai. Tài khoản đã bị khóa do nhập sai liên tiếp 5 lần."
                            };
                        }

                        int remaining = 5 - account.FailedLoginCount;
                        return new LoginResultViewModel
                        {
                            Success = false,
                            Message = $"Mật khẩu không chính xác. Bạn còn {remaining} lần thử trước khi bị khóa."
                        };
                    }

                    account.FailedLoginCount = 0;
                    account.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    string tokenValue = Guid.NewGuid().ToString();

                    string displayName = account.Email;
                    if (account.CenterManager != null && !string.IsNullOrWhiteSpace(account.CenterManager.FullName))
                    {
                        displayName = account.CenterManager.FullName;
                    }
                    else if (account.Coach != null && !string.IsNullOrWhiteSpace(account.Coach.FullName))
                    {
                        displayName = account.Coach.FullName;
                    }
                    else if (account.Member != null && !string.IsNullOrWhiteSpace(account.Member.FullName))
                    {
                        displayName = account.Member.FullName;
                    }

                    string roleName = account.Role?.Name ?? "Member";

                    UserSession.AccessToken = Guid.Parse(tokenValue);
                    UserSession.AccountId = account.Id;
                    UserSession.Email = account.Email;
                    UserSession.FullName = displayName;
                    UserSession.RoleName = roleName;

                    return new LoginResultViewModel
                    {
                        Success = true,
                        Message = "Đăng nhập thành công.",
                        AccessToken = tokenValue,
                        AccountId = account.Id,
                        Email = account.Email,
                        FullName = displayName,
                        Role = roleName
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResultViewModel
                {
                    Success = false,
                    Message = $"Đã xảy ra lỗi khi đăng nhập: {ex.Message}"
                };
            }
            finally
            {
            }
        }

        public async Task<CheckTokenResultViewModel?> CheckTokenAsync(string tokenValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tokenValue) || !Guid.TryParse(tokenValue, out Guid parsedGuid))
                {
                    return null;
                }

                if (!UserSession.IsLoggedIn || UserSession.AccessToken != parsedGuid)
                {
                    return null;
                }

                return await Task.FromResult(new CheckTokenResultViewModel
                {
                    AccountId = UserSession.AccountId,
                    Email = UserSession.Email,
                    Role = UserSession.RoleName,
                    FullName = UserSession.FullName
                });
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
            }
        }
    }
}
