using Microsoft.EntityFrameworkCore;
using SportCenterManament_PRN.DataAccess.Entities;
using SportCenterManament_PRN.Services.AccountService;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Tests;

public sealed class AuthServiceTests : IDisposable
{
    public AuthServiceTests() => UserSession.Clear();

    [Fact]
    public async Task LoginAsync_WithValidCredentials_NormalizesEmailAndCreatesSession()
    {
        var fixture = await CreateFixtureAsync();

        var result = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = "  TEST.MEMBER@EXAMPLE.COM ",
            Password = "Correct-password-123!"
        });

        Assert.True(result.Success);
        Assert.Equal("Member", result.Role);
        Assert.Equal("Test Member", result.FullName);
        Assert.True(Guid.TryParse(result.AccessToken, out _));
        Assert.True(UserSession.IsLoggedIn);
        Assert.Equal("member-account", UserSession.AccountId);
    }

    [Fact]
    public async Task LoginAsync_WithEmptyEmailOrPassword_FailsWithoutDatabaseMutation()
    {
        var fixture = await CreateFixtureAsync();

        var missingEmail = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = " ",
            Password = "some-password"
        });
        var missingPassword = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = "test.member@example.com",
            Password = ""
        });

        Assert.False(missingEmail.Success);
        Assert.False(missingPassword.Success);
        Assert.Equal(0, (await GetAccountAsync(fixture)).FailedLoginCount);
        Assert.False(UserSession.IsLoggedIn);
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_IncrementsFailedAttemptCount()
    {
        var fixture = await CreateFixtureAsync();

        var result = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = "test.member@example.com",
            Password = "wrong-password"
        });

        Assert.False(result.Success);
        Assert.Contains("còn 4 lần", result.Message);
        Assert.Equal(1, (await GetAccountAsync(fixture)).FailedLoginCount);
        Assert.False((await GetAccountAsync(fixture)).IsLocked);
    }

    [Fact]
    public async Task LoginAsync_AfterFiveWrongPasswords_LocksAccount()
    {
        var fixture = await CreateFixtureAsync();

        for (var attempt = 0; attempt < 5; attempt++)
        {
            await fixture.Service.LoginAsync(new LoginViewModel
            {
                Email = "test.member@example.com",
                Password = "wrong-password"
            });
        }

        var account = await GetAccountAsync(fixture);
        Assert.Equal(5, account.FailedLoginCount);
        Assert.True(account.IsLocked);

        var lockedLogin = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = "test.member@example.com",
            Password = "Correct-password-123!"
        });
        Assert.False(lockedLogin.Success);
        Assert.Contains("đã bị khóa", lockedLogin.Message);
    }

    [Theory]
    [InlineData("Inactive", false, false, "ngừng hoạt động")]
    [InlineData("Active", true, false, "đã bị khóa")]
    [InlineData("Active", false, true, "đã bị xóa")]
    public async Task LoginAsync_WhenAccountIsUnavailable_RejectsLogin(
        string status, bool locked, bool deleted, string expectedMessage)
    {
        var fixture = await CreateFixtureAsync(status, locked, deleted);

        var result = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = "test.member@example.com",
            Password = "Correct-password-123!"
        });

        Assert.False(result.Success);
        Assert.Contains(expectedMessage, result.Message);
        Assert.False(UserSession.IsLoggedIn);
    }

    [Fact]
    public async Task LoginByRoleAsync_RejectsAccountWithDifferentRole()
    {
        var fixture = await CreateFixtureAsync();

        var result = await fixture.Service.LoginByRoleAsync(new LoginViewModel
        {
            Email = "test.member@example.com",
            Password = "Correct-password-123!"
        }, "Coach");

        Assert.False(result.Success);
        Assert.Contains("Coach", result.Message);
        Assert.Equal(0, (await GetAccountAsync(fixture)).FailedLoginCount);
    }

    [Fact]
    public async Task LoginAsync_WithUnknownEmail_FailsWithoutCreatingSession()
    {
        var fixture = await CreateFixtureAsync();

        var result = await fixture.Service.LoginAsync(new LoginViewModel
        {
            Email = "missing@example.com",
            Password = "some-password"
        });

        Assert.False(result.Success);
        Assert.Contains("không chính xác", result.Message);
        Assert.False(UserSession.IsLoggedIn);
    }

    private static async Task<TestFixture> CreateFixtureAsync(
        string status = "Active", bool isLocked = false, bool isDeleted = false)
    {
        var databaseName = $"auth-tests-{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<SportsCenterDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        await using (var context = new SportsCenterDbContext(options))
        {
            var role = new Role { Id = 3, Name = "Member" };
            context.Roles.Add(role);
            context.Accounts.Add(new Account
            {
                Id = "member-account",
                Email = "test.member@example.com",
                PasswordHash = HashService.GenerateSHA512("Correct-password-123!"),
                Status = status,
                FailedLoginCount = 0,
                IsLocked = isLocked,
                CreatedAt = DateTime.UtcNow,
                RoleId = role.Id,
                Role = role,
                DeletedAt = isDeleted ? DateTime.UtcNow : null,
                Member = new Member
                {
                    AccountId = "member-account",
                    MemberCode = "TEST-001",
                    FullName = "Test Member",
                    CreatedAt = DateTime.UtcNow
                }
            });
            await context.SaveChangesAsync();
        }

        var service = new AuthService(() => new SportsCenterDbContext(options));
        return new TestFixture(options, service);
    }

    private static async Task<Account> GetAccountAsync(TestFixture fixture)
    {
        await using var context = new SportsCenterDbContext(fixture.Options);
        return await context.Accounts.SingleAsync(account => account.Id == "member-account");
    }

    public void Dispose() => UserSession.Clear();

    private sealed record TestFixture(
        DbContextOptions<SportsCenterDbContext> Options,
        AuthService Service);
}
