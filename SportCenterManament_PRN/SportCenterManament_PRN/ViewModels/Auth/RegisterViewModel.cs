using System.ComponentModel.DataAnnotations;

namespace SportCenterManament_PRN.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Username.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Username phải từ 3 đến 30 ký tự.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Họ và tên.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu tối thiểu 6 ký tự.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận Mật khẩu.")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public int RoleId { get; set; } = 3;
    }
}
