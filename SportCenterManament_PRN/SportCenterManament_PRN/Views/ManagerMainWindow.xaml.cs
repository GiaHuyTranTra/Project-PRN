using System.Windows;
using SportCenterManament_PRN.Services.AuthService;

namespace SportCenterManament_PRN.Views
{
    public partial class ManagerMainWindow : Window
    {
        public ManagerMainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn || !UserSession.IsCenterManager())
            {
                UserSession.Clear();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
                return;
            }

            txtUserName.Text = UserSession.FullName;
            txtUserRole.Text = UserSession.RoleName;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Clear();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }
    }
}
