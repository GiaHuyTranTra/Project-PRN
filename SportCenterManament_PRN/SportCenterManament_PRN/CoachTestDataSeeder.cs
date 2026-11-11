
using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenterManament_PRN.SeedData
{
    public static class CoachTestDataSeeder
    {
        public static async Task SeedAsync()
        {
            await using var db = new SportsCenterDbContext();

            // 1. LẤY TÀI KHOẢN COACH
            var coachRole = await db.Roles
                .FirstOrDefaultAsync(r => r.Name == "Coach");

            if (coachRole == null)
                throw new Exception("Không tìm thấy Role Coach.");

            var account = await db.Accounts
                .FirstOrDefaultAsync(a =>
                    a.RoleId == coachRole.Id &&
                    a.Status == "Active" &&
                    !a.IsLocked &&
                    a.DeletedAt == null);

            if (account == null)
                throw new Exception(
                    "Chưa có tài khoản Coach đang hoạt động.");

            // 2. TẠO COACH
            var coach = await db.Coaches
                .FirstOrDefaultAsync(c =>
                    c.AccountId == account.Id);

            if (coach == null)
            {
                coach = new Coach
                {
                    AccountId = account.Id,
                    FullName = "Nguyen Van Coach",
                    Specialization = "Yoga, Gym",
                    CreatedAt = DateTime.UtcNow
                };

                db.Coaches.Add(coach);
                await db.SaveChangesAsync();
            }

            // 3. TẠO BỘ MÔN TEST NẾU CHƯA CÓ
            var yoga = await db.Disciplines
                .FirstOrDefaultAsync(d => d.Name == "Yoga");

            if (yoga == null)
            {
                yoga = new Discipline
                {
                    Name = "Yoga",
                    Description = "Bộ môn Yoga",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                db.Disciplines.Add(yoga);
                await db.SaveChangesAsync();
            }

            // 4. GÁN COACH CHO BỘ MÔN YOGA
            bool coachDisciplineExists =
                await db.CoachDisciplines.AnyAsync(cd =>
                    cd.CoachAccountId == coach.AccountId &&
                    cd.DisciplineId == yoga.Id);

            if (!coachDisciplineExists)
            {
                db.CoachDisciplines.Add(new CoachDiscipline
                {
                    CoachAccountId = coach.AccountId,
                    DisciplineId = yoga.Id,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // 5. TẠO PHÒNG TẬP TEST
            var room = await db.Rooms
                .FirstOrDefaultAsync(r =>
                    r.Name == "Room Test Yoga");

            if (room == null)
            {
                db.Rooms.Add(new Room
                {
                    Name = "Room Test Yoga",
                    Location = "Tầng 1",
                    Capacity = 30,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await db.SaveChangesAsync();
        }
    }
}
