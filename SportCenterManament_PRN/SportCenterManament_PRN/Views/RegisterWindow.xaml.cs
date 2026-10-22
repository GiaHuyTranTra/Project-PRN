using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SportCenterManament_PRN.Services.AccountService;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Views
{
    public partial class RegisterWindow : Window
    {
        private readonly IAccountService _accountService;

        public RegisterWindow()
        {
            InitializeComponent();
            _accountService = new AccountService();
            txtUsername.Focus();
        }

        private async void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            bdrMessage.Visibility = Visibility.Collapsed;
            lblMessage.Text = string.Empty;

            string username = txtUsername.Text.Trim();
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowMessage("Vui lòng nhập Username.", false);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowMessage("Vui lòng nhập Họ và tên.", false);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowMessage("Vui lòng nhập địa chỉ Email.", false);
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("Vui lòng nhập Mật khẩu.", false);
                txtPassword.Focus();
                return;
            }

            if (password.Length < 6)
            {
                ShowMessage("Mật khẩu phải có tối thiểu 6 ký tự.", false);
                txtPassword.Focus();
                return;
            }

            if (password != confirmPassword)
            {
                ShowMessage("Mật khẩu xác nhận không khớp.", false);
                txtConfirmPassword.Focus();
                return;
            }

            int roleId = 3;
            ComboBoxItem? selectedItem = cboRole.SelectedItem as ComboBoxItem;
            if (selectedItem != null && selectedItem.Tag != null)
            {
                int.TryParse(selectedItem.Tag.ToString(), out roleId);
            }

            SetLoading(true);

            try
            {
                RegisterViewModel registerInfo = new RegisterViewModel
                {
                    Username = username,
                    FullName = fullName,
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword,
                    Phone = string.IsNullOrWhiteSpace(phone) ? null : phone,
                    RoleId = roleId
                };

                (bool Success, string Message) result = await _accountService.RegisterAccountAsync(registerInfo);

                if (result.Success)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công!\nHãy sử dụng Email vừa tạo để đăng nhập vào hệ thống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.txtEmail.Text = email;
                    loginWindow.txtPassword.Focus();
                    loginWindow.Show();

                    this.Close();
                }
                else
                {
                    ShowMessage(result.Message, false);
                    SetLoading(false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Lỗi hệ thống: {ex.Message}", false);
                SetLoading(false);
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void SetLoading(bool isLoading)
        {
            btnRegister.IsEnabled = !isLoading;
            btnBackToLogin.IsEnabled = !isLoading;
            txtUsername.IsEnabled = !isLoading;
            txtFullName.IsEnabled = !isLoading;
            txtEmail.IsEnabled = !isLoading;
            txtPassword.IsEnabled = !isLoading;
            txtConfirmPassword.IsEnabled = !isLoading;
            txtPhone.IsEnabled = !isLoading;
            cboRole.IsEnabled = !isLoading;
            prgLoading.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            if (isSuccess)
            {
                bdrMessage.Background = new SolidColorBrush(Color.FromRgb(220, 252, 231));
                bdrMessage.BorderBrush = new SolidColorBrush(Color.FromRgb(74, 222, 128));
                lblMessage.Foreground = new SolidColorBrush(Color.FromRgb(21, 128, 61));
            }
            else
            {
                bdrMessage.Background = new SolidColorBrush(Color.FromRgb(254, 226, 226));
                bdrMessage.BorderBrush = new SolidColorBrush(Color.FromRgb(248, 113, 113));
                lblMessage.Foreground = new SolidColorBrush(Color.FromRgb(185, 28, 28));
            }
            bdrMessage.Visibility = Visibility.Visible;
        }
    }
}
