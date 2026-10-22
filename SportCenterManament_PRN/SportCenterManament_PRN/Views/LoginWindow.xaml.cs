using System;
using System.Threading.Tasks;
using System.Windows;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.ViewModels.Auth;

namespace SportCenterManament_PRN.Views
{
    public partial class LoginWindow : Window
    {
        private readonly IAuthService _authService;

        public LoginWindow()
        {
            InitializeComponent();
            _authService = new AuthService();
            txtEmail.Focus();
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bdrError.Visibility = Visibility.Collapsed;
            lblErrorMessage.Text = string.Empty;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowErrorMessage("Vui lòng nhập địa chỉ Email.");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowErrorMessage("Vui lòng nhập Mật khẩu.");
                txtPassword.Focus();
                return;
            }

            SetLoadingState(true);

            try
            {
                LoginViewModel loginInfo = new LoginViewModel
                {
                    Email = email,
                    Password = password
                };

                LoginResultViewModel result = await _authService.LoginCenterManagerAsync(loginInfo);

                if (result.Success)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    this.Close();
                }
                else
                {
                    ShowErrorMessage(result.Message);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    SetLoadingState(false);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi hệ thống khi đăng nhập: {ex.Message}");
                SetLoadingState(false);
            }
            finally
            {
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            if (isLoading)
            {
                btnLogin.IsEnabled = false;
                txtEmail.IsEnabled = false;
                txtPassword.IsEnabled = false;
                prgLoading.Visibility = Visibility.Visible;
            }
            else
            {
                btnLogin.IsEnabled = true;
                txtEmail.IsEnabled = true;
                txtPassword.IsEnabled = true;
                prgLoading.Visibility = Visibility.Collapsed;
            }
        }

        private void ShowErrorMessage(string message)
        {
            lblErrorMessage.Text = message;
            bdrError.Visibility = Visibility.Visible;
        }
    }
}
