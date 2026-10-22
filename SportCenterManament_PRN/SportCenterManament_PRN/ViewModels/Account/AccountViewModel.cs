using System;

namespace SportCenterManament_PRN.ViewModels.Account
{
    public class AccountViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Status { get; set; } = "Active";
        public string RoleName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsLocked { get; set; }
    }
}
