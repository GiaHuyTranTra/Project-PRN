using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;

namespace SportCenterManament_PRN.Services.MemberListService
{
    public class MemberListService
    {
        public async Task<MemberPageResult> GetMembersAsync(
            int page,
            int pageSize,
            string search,
            string status)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 20;
            }

            await using SportsCenterDbContext context = new SportsCenterDbContext();

            IQueryable<Member> query = context.Members
                .AsNoTracking()
                .Include(member => member.Account)
                .ThenInclude(account => account.Role)
                .Where(member =>
                    member.Account.DeletedAt == null &&
                    member.Account.Role.Name == "Member");

            if (!string.IsNullOrWhiteSpace(search))
            {
                string keyword = search.Trim();

                query = query.Where(member =>
                    member.MemberCode.Contains(keyword) ||
                    (member.FullName != null && member.FullName.Contains(keyword)) ||
                    member.Account.Email.Contains(keyword) ||
                    (member.Account.Phone != null && member.Account.Phone.Contains(keyword)));
            }

            if (string.Equals(status, "Active", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(member => member.Account.Status == "Active");
            }
            else if (string.Equals(status, "Inactive", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(member => member.Account.Status != "Active");
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (totalPages == 0)
            {
                totalPages = 1;
            }

            if (page > totalPages)
            {
                page = totalPages;
            }

            var members = await query
                .OrderByDescending(member => member.CreatedAt)
                .ThenBy(member => member.MemberCode)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(member => new MemberListItem
                {
                    AccountId = member.AccountId,
                    MemberCode = member.MemberCode,
                    FullName = member.FullName ?? string.Empty,
                    Email = member.Account.Email,
                    Phone = member.Account.Phone ?? string.Empty,
                    Status = member.Account.Status
                })
                .ToListAsync();

            return new MemberPageResult
            {
                Items = members,
                TotalItems = totalItems,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
    }
}
