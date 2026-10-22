using System.Windows;
using SportCenterManament_PRN.Services.AuthService;

namespace SportCenterManament_PRN.Views
{
    public partial class CustomerMainWindow : Window
    {
        public CustomerMainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn)
            {
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
