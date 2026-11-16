using SportCenterManament_PRN.Services.AccountService;

namespace SportCenterManament_PRN.Tests;

public sealed class HashServiceTests
{
    [Fact]
    public void VerifyPassword_AcceptsCorrectBCryptPassword()
    {
        var hash = HashService.HashPasswordBCrypt("correct-password");

        Assert.True(HashService.VerifyPassword("correct-password", hash));
        Assert.False(HashService.VerifyPassword("incorrect-password", hash));
    }

    [Fact]
    public void VerifyPassword_AcceptsLegacySha512Hash()
    {
        var hash = HashService.GenerateSHA512("legacy-password");

        Assert.True(HashService.VerifyPassword("legacy-password", hash));
        Assert.False(HashService.VerifyPassword("another-password", hash));
    }

    [Theory]
    [InlineData("", "hash")]
    [InlineData("password", "")]
    public void VerifyPassword_RejectsEmptyInput(string password, string hash)
    {
        Assert.False(HashService.VerifyPassword(password, hash));
    }
}
