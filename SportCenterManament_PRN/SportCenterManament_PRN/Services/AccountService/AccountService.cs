using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.ViewModels.Account;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Services.AccountService
{
    public class AccountService : IAccountService
    {
        public async Task<bool> IsDuplicateAsync(string email, string phone)
        {
            try
            {
                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    string normalizedEmail = email.Trim().ToLower();
                    string normalizedPhone = phone.Trim();

                    Account? account = await context.Accounts
                        .AsNoTracking()
                        .FirstOrDefaultAsync(q => q.Email.ToLower() == normalizedEmail || (q.Phone != null && q.Phone == normalizedPhone));

                    return account != null;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<(bool Success, string Message)> RegisterAccountAsync(RegisterViewModel info)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(info.Username))
                {
                    return (false, "Vui lòng nhập Username.");
                }

                if (string.IsNullOrWhiteSpace(info.Email))
                {
                    return (false, "Vui lòng nhập Email.");
                }

                if (string.IsNullOrWhiteSpace(info.Password))
                {
                    return (false, "Vui lòng nhập Mật khẩu.");
                }

                string normalizedEmail = info.Email.Trim().ToLower();
                string normalizedUsername = info.Username.Trim();
                string? normalizedPhone = string.IsNullOrWhiteSpace(info.Phone) ? null : info.Phone.Trim();

                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    bool emailExists = await context.Accounts.AnyAsync(a => a.Email.ToLower() == normalizedEmail);
                    if (emailExists)
                    {
                        return (false, "Địa chỉ Email này đã được sử dụng.");
                    }

                    if (normalizedPhone != null)
                    {
                        bool phoneExists = await context.Accounts.AnyAsync(a => a.Phone == normalizedPhone);
                        if (phoneExists)
                        {
                            return (false, "Số điện thoại này đã được sử dụng.");
                        }
                    }

                    if (info.RoleId == 3)
                    {
                        bool usernameExists = await context.Members.AnyAsync(m => m.MemberCode.ToLower() == normalizedUsername.ToLower());
                        if (usernameExists)
                        {
                            return (false, "Username này đã tồn tại, vui lòng chọn Username khác.");
                        }
                    }

                    Role? role = await context.Roles.FirstOrDefaultAsync(r => r.Id == info.RoleId);
                    if (role == null)
                    {
                        role = await context.Roles.FirstOrDefaultAsync(r => r.Id == 3);
                        if (role == null)
                        {
                            return (false, "Không tìm thấy vai trò tài khoản hợp lệ.");
                        }
                    }

                    string newAccountId = Guid.NewGuid().ToString();
                    string hashedPassword = HashService.HashPasswordBCrypt(info.Password);

                    Account newAccount = new Account
                    {
                        Id = newAccountId,
                        Email = normalizedEmail,
                        PasswordHash = hashedPassword,
                        Phone = normalizedPhone,
                        RoleId = role.Id,
                        Status = "Active",
                        FailedLoginCount = 0,
                        IsLocked = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    await context.Accounts.AddAsync(newAccount);

                    if (role.Id == 1)
                    {
                        CenterManager centerManager = new CenterManager
                        {
                            AccountId = newAccountId,
                            FullName = info.FullName.Trim(),
                            CreatedAt = DateTime.UtcNow
                        };
                        await context.CenterManagers.AddAsync(centerManager);
                    }
                    else if (role.Id == 2)
                    {
                        Coach coach = new Coach
                        {
                            AccountId = newAccountId,
                            FullName = info.FullName.Trim(),
                            CreatedAt = DateTime.UtcNow
                        };
                        await context.Coaches.AddAsync(coach);
                    }
                    else
                    {
                        Member member = new Member
                        {
                            AccountId = newAccountId,
                            MemberCode = normalizedUsername,
                            FullName = info.FullName.Trim(),
                            CreatedAt = DateTime.UtcNow
                        };
                        await context.Members.AddAsync(member);
                    }

                    await context.SaveChangesAsync();
                    return (true, "Đăng ký tài khoản thành công.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi khi đăng ký tài khoản: {ex.Message}");
            }
        }

        public async Task<bool> CreateCenterManagerAsync(AccountViewModel info)
        {
            try
            {
                if (await IsDuplicateAsync(info.Email, info.Phone ?? string.Empty))
                {
                    return false;
                }

                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Role? role = await context.Roles.FirstOrDefaultAsync(q => q.Name == "CenterManager" || q.Id == 1);
                    if (role == null)
                    {
                        return false;
                    }

                    string newAccountId = Guid.NewGuid().ToString();
                    string hashedPassword = HashService.HashPasswordBCrypt(info.Password);

                    Account newAccount = new Account
                    {
                        Id = newAccountId,
                        Email = info.Email.Trim().ToLower(),
                        PasswordHash = hashedPassword,
                        Phone = string.IsNullOrWhiteSpace(info.Phone) ? null : info.Phone.Trim(),
                        RoleId = role.Id,
                        Status = "Active",
                        FailedLoginCount = 0,
                        IsLocked = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    await context.Accounts.AddAsync(newAccount);

                    CenterManager newCenterManager = new CenterManager
                    {
                        AccountId = newAccountId,
                        FullName = info.FullName.Trim(),
                        CreatedAt = DateTime.UtcNow
                    };

                    await context.CenterManagers.AddAsync(newCenterManager);
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            try
            {
                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    string normalizedEmail = email.Trim().ToLower();
                    Account? account = await context.Accounts
                        .AsNoTracking()
                        .Include(a => a.Role)
                        .Include(a => a.CenterManager)
                        .Include(a => a.Coach)
                        .Include(a => a.Member)
                        .FirstOrDefaultAsync(a => a.Email.ToLower() == normalizedEmail);

                    return account;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> LockAccountAsync(string accountId)
        {
            try
            {
                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Account? account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                    if (account == null)
                    {
                        return false;
                    }

                    account.IsLocked = true;
                    account.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnlockAccountAsync(string accountId)
        {
            try
            {
                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Account? account = await context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                    if (account == null)
                    {
                        return false;
                    }

                    account.IsLocked = false;
                    account.FailedLoginCount = 0;
                    account.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UnlockAccountByEmailAsync(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return false;
                }

                string normalizedEmail = email.Trim().ToLower();

                await using (SportsCenterDbContext context = new SportsCenterDbContext())
                {
                    Account? account = await context.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == normalizedEmail);
                    if (account == null)
                    {
                        return false;
                    }

                    account.IsLocked = false;
                    account.FailedLoginCount = 0;
                    account.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
