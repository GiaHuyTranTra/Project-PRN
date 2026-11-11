
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

        // Chỉ bật true khi test cục bộ.
        // Trước khi triển khai thực tế, chuyển thành false.
        private const bool AllowTestWithoutLogin = false;

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

        private bool HasPermission()
        {
            if (!UserSession.IsLoggedIn)
            {
                return false;
            }

            return UserSession.IsCenterManager() ||
                   string.Equals(
                       UserSession.RoleName?.Trim(),
                       "Admin",
                       StringComparison.OrdinalIgnoreCase);
        }

        private async void Window_Loaded(
            object sender,
            RoutedEventArgs e)
        {
            if (!HasPermission())
            {
                MessageBox.Show(
                    "Bạn không có quyền phân công Coach.");
                Close();
                return;
            }

            try
            {
                cbCoach.ItemsSource =
                    await _service.GetCoachesAsync(
                        _course.DisciplineId);

                cbRoom.ItemsSource =
                    await _service.GetRoomsAsync();

                cbCoach.DisplayMemberPath = "Name";
                cbCoach.SelectedValuePath = "Id";

                cbRoom.DisplayMemberPath = "Name";
                cbRoom.SelectedValuePath = "Id";

                if (cbCoach.Items.Count > 0)
                {
                    cbCoach.SelectedIndex = 0;
                }

                if (cbRoom.Items.Count > 0)
                {
                    cbRoom.SelectedIndex = 0;
                }

                if (cbCoach.Items.Count == 0)
                {
                    MessageBox.Show(
                        "Chưa có Coach phù hợp với bộ môn của lớp.");
                }

                if (cbRoom.Items.Count == 0)
                {
                    MessageBox.Show(
                        "Chưa có phòng tập đang hoạt động.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không tải được danh sách: " +
                    ex.GetBaseException().Message);
            }
        }

        private async void btnSave_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!HasPermission())
            {
                MessageBox.Show(
                    "Bạn không có quyền phân công Coach.");
                return;
            }

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

            if (start >= end)
            {
                MessageBox.Show(
                    "Giờ kết thúc phải lớn hơn giờ bắt đầu.");
                return;
            }

            if (capacity > _course.Capacity)
            {
                MessageBox.Show(
                    "Sức chứa buổi học không được vượt quá " +
                    "sức chứa của lớp.");
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
                var result =
                    await _service.AssignCoachAsync(model);

                MessageBox.Show(
                    result.Message,
                    result.Success ? "Thành công" : "Thông báo",
                    MessageBoxButton.OK,
                    result.Success
                        ? MessageBoxImage.Information
                        : MessageBoxImage.Warning);

                if (result.Success)
                {
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi lưu phân công: " +
                    ex.GetBaseException().Message);
            }
            finally
            {
                btnSave.IsEnabled = true;
            }
        }

        private void btnCancel_Click(
            object sender,
            RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
