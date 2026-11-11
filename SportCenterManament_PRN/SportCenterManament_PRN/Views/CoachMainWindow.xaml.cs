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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn || !UserSession.IsCoach())
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

        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            FeedbackWindow feedbackWindow = new FeedbackWindow
            {
                Owner = this
            };
            feedbackWindow.ShowDialog();
        }

        private void btnAttendance_Click(object sender, RoutedEventArgs e)
        {
            AttendanceWindow attendanceWindow = new AttendanceWindow
            {
                Owner = this
            };
            attendanceWindow.ShowDialog();
        }
    }
}
