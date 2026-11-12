using SportCenterManament_PRN.Services.CoachDashboardService;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SportCenterManament_PRN.Views
{
    public partial class CoachDashboardView : UserControl
    {
        private readonly ICoachDashboardService _service =
            new CoachDashboardService();

        private bool _initialized = false;

        public CoachDashboardView()
        {
            InitializeComponent();
        }

        private async void UserControl_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            if (_initialized)
                return;

            _initialized = true;

            try
            {
                await LoadDashboardAsync();
            }
            catch (Exception ex)
            {
                _initialized = false;

                MessageBox.Show(
                    "Không thể tải Coach Dashboard:\n" +
                    ex.GetBaseException().Message,
                    "Lỗi",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task LoadDashboardAsync()
        {
            // THÔNG TIN CÁ NHÂN
            var profile = await _service.GetMyProfileAsync();

            if (profile == null)
            {
                MessageBox.Show(
                    "Không tìm thấy thông tin Coach của tài khoản này.");
                return;
            }

            txtFullName.Text =
                "Họ tên: " + profile.FullName;

            txtEmail.Text =
                "Email: " + profile.Email;

            txtPhone.Text =
                "Số điện thoại: " + profile.Phone;

            txtSpecialization.Text =
                "Chuyên môn: " + profile.Specialization;

            txtDisciplines.Text =
                "Bộ môn phụ trách: " + profile.DisciplineNames;

            txtWorkSchedule.Text =
                "Lịch làm việc: " + profile.WorkSchedule;

            // DANH SÁCH LỚP
            dgClasses.ItemsSource =
                await _service.GetMyClassesAsync();

            // LỊCH DẠY
            dgSchedule.ItemsSource =
                await _service.GetMyScheduleAsync();
        }
    }
}