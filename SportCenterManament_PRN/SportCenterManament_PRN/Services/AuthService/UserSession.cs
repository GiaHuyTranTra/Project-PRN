using System;

namespace SportCenterManament_PRN.Services.AuthService
{
    public static class UserSession
    {
        public static Guid? AccessToken { get; set; }
        public static string AccountId { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string FullName { get; set; } = string.Empty;
        public static string RoleName { get; set; } = string.Empty;
        public static bool IsLoggedIn => AccessToken.HasValue;

        public static void Clear()
        {
            AccessToken = null;
            AccountId = string.Empty;
            Email = string.Empty;
            FullName = string.Empty;
            RoleName = string.Empty;
        }
    }
}
