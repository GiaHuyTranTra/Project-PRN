using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using SportCenterManament_PRN.Services.AttendanceService;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.ViewModels.Attendance;

namespace SportCenterManament_PRN.Views;

public partial class AttendanceWindow : Window
{
    private readonly AttendanceService attendanceService = new AttendanceService();
    private readonly ObservableCollection<AttendanceRowViewModel> rows =
        new ObservableCollection<AttendanceRowViewModel>();
    private bool isLoadingSessions;

    public AttendanceWindow()
    {
        InitializeComponent();
        dgAttendance.ItemsSource = rows;
        dpSessionDate.SelectedDate = DateTime.Today;
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (!UserSession.IsLoggedIn ||
            (!UserSession.IsCoach() && !UserSession.IsCenterManager()))
        {
            MessageBox.Show("Bạn không có quyền truy cập chức năng điểm danh.",
                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            Close();
            return;
        }

        await LoadSessionsAsync();
    }

    private async Task LoadSessionsAsync()
    {
        if (isLoadingSessions || dpSessionDate.SelectedDate == null)
        {
            return;
        }

        try
        {
            isLoadingSessions = true;
            cbSessions.ItemsSource = null;
            rows.Clear();

            DateOnly selectedDate = DateOnly.FromDateTime(dpSessionDate.SelectedDate.Value);
            List<AttendanceSessionViewModel> sessions =
                await attendanceService.GetSessionsAsync(selectedDate);

            cbSessions.ItemsSource = sessions;
            txtStatus.Text = $"Tìm thấy {sessions.Count} buổi học.";

            if (sessions.Count > 0)
            {
                cbSessions.SelectedIndex = 0;
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Lỗi tải lịch",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            isLoadingSessions = false;
        }
    }

    private async Task LoadAttendanceAsync()
    {
        if (cbSessions.SelectedItem is not AttendanceSessionViewModel selected)
        {
            rows.Clear();
            return;
        }

        try
        {
            List<AttendanceRowViewModel> attendanceRows =
                await attendanceService.GetAttendanceRowsAsync(selected.SessionId);

            rows.Clear();
            foreach (AttendanceRowViewModel row in attendanceRows)
            {
                rows.Add(row);
            }

            txtStatus.Text = $"{selected.ClassName}: {rows.Count} học viên đăng ký.";
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Lỗi tải học viên",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void dpSessionDate_SelectedDateChanged(
        object sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded)
        {
            await LoadSessionsAsync();
        }
    }

    private async void cbSessions_SelectionChanged(
        object sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded && !isLoadingSessions)
        {
            await LoadAttendanceAsync();
        }
    }

    private async void btnLoad_Click(object sender, RoutedEventArgs e)
    {
        await LoadAttendanceAsync();
    }

    private async void btnSave_Click(object sender, RoutedEventArgs e)
    {
        if (cbSessions.SelectedItem is not AttendanceSessionViewModel selected)
        {
            MessageBox.Show("Vui lòng chọn buổi học.");
            return;
        }

        try
        {
            dgAttendance.CommitEdit(DataGridEditingUnit.Cell, true);
            dgAttendance.CommitEdit(DataGridEditingUnit.Row, true);

            await attendanceService.SaveAttendanceAsync(selected.SessionId, rows.ToList());

            txtStatus.Text = "Đã lưu điểm danh thành công.";
            MessageBox.Show("Lưu điểm danh thành công.", "Thông báo",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Lỗi lưu điểm danh",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void btnReport_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            List<AttendanceReportViewModel> reports =
                await attendanceService.GetAttendanceReportAsync();

            string reportText = reports.Count == 0
                ? "Chưa có dữ liệu điểm danh."
                : string.Join(Environment.NewLine, reports.Select(report =>
                    $"{report.ClassName}: {report.PresentCount}/{report.TotalMarked} có mặt " +
                    $"({report.AttendanceRate:F1}%), vắng {report.AbsentCount}"));

            MessageBox.Show(reportText, "Báo cáo tỷ lệ chuyên cần",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message, "Lỗi báo cáo",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
