
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
            if (!UserSession.IsLoggedIn ||
                !UserSession.IsCenterManager())
            {
                MessageBox.Show(
                    "Bạn không có quyền truy cập trang quản lý.");

                new LoginWindow().Show();
                Close();
                return;
            }

            txtFullName.Text = UserSession.FullName;
            txtRole.Text = UserSession.RoleName;

            // Hiển thị trang quản lý lớp học mặc định
            mainContent.Content = new CourseManagementView();
        }

        private void btnCourseManagement_Click(
            object sender,
            RoutedEventArgs e)
        {
            mainContent.Content = new CourseManagementView();
        }

        private void btnLogout_Click(
            object sender,
            RoutedEventArgs e)
        {
            UserSession.Clear();

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            Close();
        }
    }
}
