using System.Windows;
using SportCenterManament_PRN.Services.AuthService;

namespace SportCenterManament_PRN.Views
{
    public partial class CoachMainWindow : Window
    {
        public CoachMainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn ||
                !UserSession.IsCoach())
            {
                MessageBox.Show(
                    "Chỉ Coach mới được truy cập trang này.");

                new LoginWindow().Show();
                Close();
                return;
            }

            txtFullName.Text = UserSession.FullName;

            mainContent.Content =
                new CoachDashboardView();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSession.Clear();

            new LoginWindow().Show();

            Close();
        }
    }
}