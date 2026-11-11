using System.Windows;
using System.Windows.Controls;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.Services.FeedbackService;
using SportCenterManament_PRN.ViewModels.Feedback;

namespace SportCenterManament_PRN.Views;

public partial class FeedbackWindow : Window
{
    private readonly IFeedbackService feedbackService = new FeedbackService();

    public FeedbackWindow()
    {
        InitializeComponent();
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        if (!UserSession.IsLoggedIn ||
            (!UserSession.IsMember() &&
             !UserSession.IsCoach() &&
             !UserSession.IsCenterManager()))
        {
            MessageBox.Show("Bạn không có quyền truy cập chức năng đánh giá.");
            Close();
            return;
        }

        if (UserSession.IsMember())
        {
            MemberPanel.Visibility = Visibility.Visible;
            await LoadMemberDataAsync();
            return;
        }

        SummaryPanel.Visibility = Visibility.Visible;
        await LoadSummaryAsync();
    }

    private async Task LoadMemberDataAsync()
    {
        try
        {
            List<FeedbackSessionOption> sessions =
                await feedbackService.GetEligibleSessionsAsync(UserSession.AccountId);
            cbSessions.ItemsSource = sessions;

            txtMemberStatus.Text = sessions.Count == 0
                ? "Chưa có buổi học đủ điều kiện đánh giá. Cần được điểm danh Có mặt trước."
                : $"Có {sessions.Count} buổi học đang chờ đánh giá.";

            List<FeedbackItemViewModel> feedback =
                await feedbackService.GetMemberFeedbackAsync(UserSession.AccountId);
            dgMyFeedback.ItemsSource = feedback;
        }
        catch (Exception exception)
        {
            txtMemberStatus.Text =
                "Không tải được dữ liệu đánh giá: " +
                exception.GetBaseException().Message;
        }
    }

    private async Task LoadSummaryAsync()
    {
        try
        {
            string? coachId = UserSession.IsCoach()
                ? UserSession.AccountId
                : null;

            int selectedRating = GetSelectedRating(cbRatingFilter);
            int? rating = selectedRating == 0 ? null : selectedRating;

            List<FeedbackAverageViewModel> averages =
                await feedbackService.GetAveragesAsync(coachId);
            List<FeedbackItemViewModel> feedback =
                await feedbackService.GetFeedbackAsync(coachId, rating);

            dgAverages.ItemsSource = averages;
            dgFeedback.ItemsSource = feedback;
            txtSummaryStatus.Text =
                $"{feedback.Count} phản hồi" +
                (UserSession.IsCoach() ? " của bạn." : " trong hệ thống.");
        }
        catch (Exception exception)
        {
            txtSummaryStatus.Text =
                "Không tải được báo cáo: " +
                exception.GetBaseException().Message;
        }
    }

    private async void btnSubmit_Click(object sender, RoutedEventArgs e)
    {
        if (cbSessions.SelectedItem is not FeedbackSessionOption session)
        {
            MessageBox.Show("Vui lòng chọn buổi học cần đánh giá.");
            return;
        }

        int rating = GetSelectedRating(cbRating);
        if (rating < 1 || rating > 5)
        {
            MessageBox.Show("Vui lòng chọn số sao từ 1 đến 5.");
            return;
        }

        (bool Success, string Message) result =
            await feedbackService.SubmitFeedbackAsync(
                UserSession.AccountId,
                session.AttendanceId,
                rating,
                txtComment.Text);

        MessageBox.Show(result.Message);
        if (result.Success)
        {
            txtComment.Clear();
            await LoadMemberDataAsync();
        }
    }

    private async void cbRatingFilter_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (IsLoaded && SummaryPanel.Visibility == Visibility.Visible)
        {
            await LoadSummaryAsync();
        }
    }

    private static int GetSelectedRating(ComboBox comboBox)
    {
        if (comboBox.SelectedItem is ComboBoxItem item &&
            int.TryParse(item.Tag?.ToString(), out int rating))
        {
            return rating;
        }

        return 0;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
