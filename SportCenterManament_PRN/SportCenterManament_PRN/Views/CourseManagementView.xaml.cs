
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.Services.CourseService;
using SportCenterManament_PRN.ViewModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace SportCenterManament_PRN.Views
{
    /// <summary>
    /// Interaction logic for CourseManagementView.xaml
    /// </summary>
    public partial class CourseManagementView : UserControl
    {

        private readonly ICourseService _service = new CourseService();
        private bool _initialized;

        public CourseManagementView()
        {
            InitializeComponent();
            LoadCoursesAsync();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
{
    if (_initialized)
        return;

    _initialized = true;

    try
    {
        var disciplines = await _service.GetDisciplinesAsync();

        // ComboBox chọn bộ môn khi thêm/sửa
        cbDiscipline.ItemsSource = disciplines;

        // ComboBox lọc bộ môn
        var filterDisciplines = new List<LookupItemViewModel>
        {
            new LookupItemViewModel
            {
                Id = "0",
                Name = "Tất cả bộ môn"
            }
        };

        filterDisciplines.AddRange(disciplines);

        cbFilterDiscipline.ItemsSource = filterDisciplines;
        cbFilterDiscipline.SelectedIndex = 0;

        cbFilterStatus.SelectedIndex = 0;

        // Hiển thị danh sách lớp học
        await LoadCoursesAsync();
    }
    catch (Exception ex)
    {
        _initialized = false;
        MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
    }
}

        private async Task LoadCoursesAsync()
        {
            string? status =
                (cbFilterStatus.SelectedItem as ComboBoxItem)?
                .Tag?.ToString();

            int? disciplineId =
                int.TryParse(
                    cbFilterDiscipline.SelectedValue?.ToString(),
                    out int id) && id > 0
                    ? id
                    : null;

            dgCourses.ItemsSource = await _service.GetCoursesAsync(
                txtSearch.Text,
                status,
                disciplineId);
        }

        private void dgCourses_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgCourses.SelectedItem is not CourseItemViewModel c)
                return;

            txtName.Text = c.Name;
            txtDescription.Text = c.Description ?? "";
            txtCapacity.Text = c.Capacity.ToString();
            cbDiscipline.SelectedValue = c.DisciplineId.ToString();
        }


        private void ClearForm()
        {
            dgCourses.SelectedItem = null;
            txtName.Clear();
            txtDescription.Clear();
            txtCapacity.Clear();
            cbDiscipline.SelectedIndex = -1;
        }


        private CourseSaveViewModel? ReadForm(int? id = null)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên lớp.");
                return null;
            }

            if (!int.TryParse(txtCapacity.Text.Trim(),
                    out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Sức chứa phải là số nguyên dương.");
                return null;
            }

            if (!int.TryParse(
                    cbDiscipline.SelectedValue?.ToString(),
                    out int disciplineId) || disciplineId <= 0)
            {
                MessageBox.Show("Vui lòng chọn bộ môn.");
                return null;
            }

            return new CourseSaveViewModel
            {
                Id = id,
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Capacity = capacity,
                DisciplineId = disciplineId
            };
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var model = ReadForm();

            if (model == null) return;

            try
            {
                var result = await _service.CreateAsync(model);

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    ClearForm();
                    await LoadCoursesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm lớp: " + ex.Message);
            }
        }

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourses.SelectedItem is not CourseItemViewModel selected)
            {
                MessageBox.Show("Vui lòng chọn lớp cần cập nhật.");
                return;
            }

            var model = ReadForm(selected.Id);

            if (model == null) return;

            try
            {
                var result = await _service.UpdateAsync(model);

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    await LoadCoursesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }

        }

        private async void btnHide_Click(object sender, RoutedEventArgs e)
        {
             if (dgCourses.SelectedItem is not CourseItemViewModel selected)
            {
                MessageBox.Show("Vui lòng chọn lớp cần ẩn.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn ẩn lớp {selected.Name}?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
                return;

            try
            {
                var result = await _service.HideAsync(selected.Id);

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    ClearForm();
                    await LoadCoursesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ẩn lớp: " + ex.Message);
            }
        }

        private async void btnAssign_Click(object sender, RoutedEventArgs e)
        {

            if (dgCourses.SelectedItem is not CourseItemViewModel selected)
            {
                MessageBox.Show("Vui lòng chọn lớp cần phân công.");
                return;
            }

            if (selected.Status != "Active")
            {
                MessageBox.Show("Không thể phân công Coach cho lớp đã ẩn.");
                return;
            }

            AssignCoachDialog dialog = new AssignCoachDialog(selected, _service);

            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();

        }

        private async void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await LoadCoursesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnRefresh_Click(
            object sender, RoutedEventArgs e)
        {
            txtSearch.Clear();
            cbFilterStatus.SelectedIndex = 0;
            cbFilterDiscipline.SelectedIndex = 0;

            try
            {
                await LoadCoursesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


    }
}
