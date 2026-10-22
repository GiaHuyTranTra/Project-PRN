using System.ComponentModel.DataAnnotations;

namespace SportCenterManament_PRN.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
        [EmailAddress(ErrorMessage = "Định dạng Email không hợp lệ.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu.")]
        public string Password { get; set; } = string.Empty;
    }
}
