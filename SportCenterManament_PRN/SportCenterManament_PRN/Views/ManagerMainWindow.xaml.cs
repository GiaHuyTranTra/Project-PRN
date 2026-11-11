using System;
using System.Windows;
using SportCenterManament_PRN.Services.AuthService;

namespace SportCenterManament_PRN.Views
{
    public partial class ManagerMainWindow : Window
    {
        // TRUE: Bỏ qua kiểm tra quyền để test
        // FALSE: Chỉ Admin và CenterManager được truy cập
        private const bool IsTesting = false
            ;

        public ManagerMainWindow()
        {
            InitializeComponent();
        }

        private bool HasPermission()
        {
            // Chế độ test: bỏ qua kiểm tra quyền
            if (IsTesting)
                return true;

            // Chế độ bình thường
            return UserSession.IsLoggedIn &&
                   (
                       UserSession.IsCenterManager() ||
                       string.Equals(
                           UserSession.RoleName?.Trim(),
                           "Admin",
                           StringComparison.OrdinalIgnoreCase)
                   );
        }

        private void Window_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            if (!HasPermission())
            {
                MessageBox.Show(
                    "Chỉ Admin và CenterManager mới được truy cập trang quản lý.");

                new LoginWindow().Show();
                Close();
                return;
            }

            // Hiển thị thông tin người dùng
            txtFullName.Text = IsTesting && !UserSession.IsLoggedIn
                ? "Test User"
                : UserSession.FullName;

            txtRole.Text = IsTesting && !UserSession.IsLoggedIn
                ? "Testing"
                : UserSession.RoleName;

            // Trang mặc định
            mainContent.Content = new CourseManagementView();
        }

        private void btnCourseManagement_Click(
            object sender,
            RoutedEventArgs e)
        {
            mainContent.Content = new CourseManagementView();
        }

        private void btnCoachManagement_Click(
            object sender,
            RoutedEventArgs e)
        {
            mainContent.Content = new CoachManagementView();
        }

        private void btnLogout_Click(
            object sender,
            RoutedEventArgs e)
        {
            UserSession.Clear();

            new LoginWindow().Show();
            Close();
        }
    }
}