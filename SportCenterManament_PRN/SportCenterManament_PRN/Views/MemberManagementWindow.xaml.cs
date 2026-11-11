using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.Services.MemberListService;

namespace SportCenterManament_PRN.Views
{
    public partial class MemberManagementWindow : Window
    {
        private readonly MemberListService _memberListService;
        private int _currentPage;
        private const int PageSize = 20;
        private string _currentSearch = string.Empty;
        private string _currentStatus = "All";
        private bool _isLoading;

        public MemberManagementWindow()
        {
            InitializeComponent();
            _memberListService = new MemberListService();
            _currentPage = 1;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UserSession.IsLoggedIn || !UserSession.IsCenterManager())
            {
                MessageBox.Show(
                    "Bạn không có quyền truy cập chức năng quản lý hội viên.",
                    "Không có quyền",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Close();
                return;
            }

            await LoadMembersAsync();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            await SearchMembersAsync();
        }

        private async void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await SearchMembersAsync();
            }
        }

        private async Task SearchMembersAsync()
        {
            _currentSearch = txtSearch.Text.Trim();

            if (cboStatus.SelectedItem is ComboBoxItem selectedItem &&
                selectedItem.Tag is string selectedStatus)
            {
                _currentStatus = selectedStatus;
            }
            else
            {
                _currentStatus = "All";
            }

            _currentPage = 1;
            await LoadMembersAsync();
        }

        private async void btnReload_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            cboStatus.SelectedIndex = 0;
            _currentSearch = string.Empty;
            _currentStatus = "All";
            _currentPage = 1;

            await LoadMembersAsync();
        }

        private async void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                await LoadMembersAsync();
            }
        }

        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            _currentPage++;
            await LoadMembersAsync();
        }

        private void dgMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Chức năng Sửa/Xóa sẽ được nối bởi thành viên phụ trách phần 4 và 5.
        }

        private async Task LoadMembersAsync()
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            SetLoadingState(true);

            try
            {
                MemberPageResult result = await _memberListService.GetMembersAsync(
                    _currentPage,
                    PageSize,
                    _currentSearch,
                    _currentStatus);

                _currentPage = result.CurrentPage;
                dgMembers.ItemsSource = result.Items;

                txtPageInfo.Text = $"Trang {_currentPage}/{result.TotalPages}";
                txtResultInfo.Text =
                    $"Tổng cộng: {result.TotalItems} hội viên | {PageSize} dòng/trang";

                btnPrevious.IsEnabled = _currentPage > 1;
                btnNext.IsEnabled = _currentPage < result.TotalPages;
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "Không thể tải danh sách hội viên. Vui lòng kiểm tra kết nối SQL Server và thử lại.",
                    "Lỗi tải dữ liệu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                dgMembers.ItemsSource = null;
                txtPageInfo.Text = "Trang 1/1";
                txtResultInfo.Text = "Không tải được dữ liệu";
                btnPrevious.IsEnabled = false;
                btnNext.IsEnabled = false;
            }
            finally
            {
                SetLoadingState(false);
                _isLoading = false;
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSearch.IsEnabled = !isLoading;
            btnReload.IsEnabled = !isLoading;
            txtSearch.IsEnabled = !isLoading;
            cboStatus.IsEnabled = !isLoading;
            btnPrevious.IsEnabled = !isLoading && _currentPage > 1;
            btnNext.IsEnabled = !isLoading;
            Mouse.OverrideCursor = isLoading ? Cursors.Wait : null;
        }
    }
}
