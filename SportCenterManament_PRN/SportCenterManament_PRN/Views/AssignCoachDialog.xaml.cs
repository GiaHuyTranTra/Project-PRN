using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.Services.CourseService;
using SportCenterManament_PRN.ViewModels.Course;
using System;
using System.Globalization;
using System.Windows;

namespace SportCenterManament_PRN.Views
{
    public partial class AssignCoachDialog : Window
    {
        private readonly CourseItemViewModel _course;
        private readonly ICourseService _service;

        public AssignCoachDialog(
            CourseItemViewModel course,
            ICourseService service)
        {
            InitializeComponent();

            _course = course;
            _service = service;

            txtClassName.Text = course.Name;
            txtCapacity.Text = course.Capacity.ToString();

            dpDate.SelectedDate = DateTime.Today;
        }

        private async void Window_Loaded(
           object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn ||
                !UserSession.IsCenterManager())
            {
                MessageBox.Show("Bạn không có quyền phân công Coach.");
                Close();
                return;
            }

            try
            {
                cbCoach.ItemsSource =
                    await _service.GetCoachesAsync(_course.DisciplineId);

                cbRoom.ItemsSource =
                    await _service.GetRoomsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không tải được danh sách: " + ex.Message);
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbCoach.SelectedValue is not string coachId ||
             !int.TryParse(
                 cbRoom.SelectedValue?.ToString(),
                 out int roomId) ||
             !dpDate.SelectedDate.HasValue ||
             !TimeOnly.TryParseExact(
                 txtStart.Text.Trim(),
                 "HH:mm",
                 CultureInfo.InvariantCulture,
                 DateTimeStyles.None,
                 out var start) ||
             !TimeOnly.TryParseExact(
                 txtEnd.Text.Trim(),
                 "HH:mm",
                 CultureInfo.InvariantCulture,
                 DateTimeStyles.None,
                 out var end) ||
             !int.TryParse(
                 txtCapacity.Text.Trim(),
                 out int capacity) ||
             capacity <= 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn Coach, phòng, ngày, " +
                    "nhập giờ HH:mm và sức chứa hợp lệ.");
                return;
            }

            var model = new AssignCoachViewModel
            {
                ClassId = _course.Id,
                CoachId = coachId,
                RoomId = roomId,
                SessionDate = DateOnly.FromDateTime(
                    dpDate.SelectedDate.Value),
                StartTime = start,
                EndTime = end,
                Capacity = capacity,
                Notes = txtNotes.Text
            };

            btnSave.IsEnabled = false;

            try
            {
                var result = await _service.AssignCoachAsync(model);

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi lưu phân công: " + ex.Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }



        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}