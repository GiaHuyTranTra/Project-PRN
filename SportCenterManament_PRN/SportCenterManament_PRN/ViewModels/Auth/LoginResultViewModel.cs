using System;

namespace SportCenterManament_PRN.ViewModels.Auth
{
    public class LoginResultViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? AccessToken { get; set; }
        public string? AccountId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
