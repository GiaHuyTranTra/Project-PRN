
using SportCenterManament_PRN.Services.AuthService;
using SportCenterManament_PRN.Services.CoachService;
using SportCenterManament_PRN.ViewModels.Coach;
using SportCenterManament_PRN.ViewModels.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SportCenterManament_PRN.Views
{
    public partial class CoachManagementView : UserControl
    {
        private readonly ICoachService _service =
            new CoachService();

        private CoachItemViewModel? _selectedCoach;

        public CoachManagementView()
        {
            InitializeComponent();
        }

        private bool CheckAdmin()
        {
            if (!UserSession.IsLoggedIn ||
                !(UserSession.IsCenterManager() ||
                  string.Equals(
                      UserSession.RoleName?.Trim(),
                      "Admin",
                      StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show(
                    "Chỉ Admin và CenterManager mới được quản lý Coach.");
                return false;
            }

            return true;
        }

        private async void UserControl_Loaded(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin())
            {
                IsEnabled = false;
                return;
            }

            try
            {
                lbDisciplines.ItemsSource =
                    await _service.GetDisciplinesAsync();

                await LoadCoachesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.GetBaseException().Message);
            }
        }

        private async System.Threading.Tasks.Task LoadCoachesAsync()
        {
            dgCoaches.ItemsSource =
                await _service.GetAllAsync(txtSearch.Text);
        }

        private List<int> GetSelectedDisciplineIds()
        {
            return lbDisciplines.SelectedItems
                .Cast<LookupItemViewModel>()
                .Select(d => int.Parse(d.Id))
                .ToList();
        }

        private CoachSaveViewModel BuildModel()
        {
            return new CoachSaveViewModel
            {
                AccountId = _selectedCoach?.AccountId,
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Password = txtPassword.Password,
                Specialization = txtSpecialization.Text.Trim(),
                WorkSchedule = txtWorkSchedule.Text.Trim(),
                DisciplineIds = GetSelectedDisciplineIds()
            };
        }

        private async void btnSearch_Click(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin()) return;
            await LoadCoachesAsync();
        }

        private async void btnRefresh_Click(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin()) return;

            txtSearch.Clear();
            await LoadCoachesAsync();
        }

        private async void dgCoaches_SelectionChanged(
            object sender, SelectionChangedEventArgs e)
        {
            if (dgCoaches.SelectedItem
                is not CoachItemViewModel coach)
                return;

            _selectedCoach = coach;

            txtFullName.Text = coach.FullName;
            txtEmail.Text = coach.Email;
            txtPhone.Text = coach.Phone ?? "";
            txtSpecialization.Text = coach.Specialization ?? "";
            txtWorkSchedule.Text = coach.WorkSchedule ?? "";
            txtPassword.Clear();

            try
            {
                var ids = await _service
                    .GetCoachDisciplineIdsAsync(coach.AccountId);

                lbDisciplines.UnselectAll();

                foreach (LookupItemViewModel item
                    in lbDisciplines.Items)
                {
                    if (int.TryParse(item.Id, out int id) &&
                        ids.Contains(id))
                    {
                        lbDisciplines.SelectedItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.GetBaseException().Message);
            }
        }

        private async void btnAdd_Click(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin()) return;

            try
            {
                var result = await _service
                    .CreateAsync(BuildModel());

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    ClearForm();
                    await LoadCoachesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.GetBaseException().Message);
            }
        }

        private async void btnUpdate_Click(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin()) return;

            if (_selectedCoach == null)
            {
                MessageBox.Show("Vui lòng chọn Coach.");
                return;
            }

            try
            {
                var result = await _service
                    .UpdateAsync(BuildModel());

                MessageBox.Show(result.Message);

                if (result.Success)
                {
                    ClearForm();
                    await LoadCoachesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.GetBaseException().Message);
            }
        }

        private async void btnLock_Click(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin()) return;

            if (_selectedCoach == null)
            {
                MessageBox.Show("Vui lòng chọn Coach.");
                return;
            }

            var confirm = MessageBox.Show(
                "Bạn có chắc muốn khóa Coach này?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes)
                return;

            var result = await _service.SetLockedAsync(
                _selectedCoach.AccountId, true);

            MessageBox.Show(result.Message);

            if (result.Success)
            {
                ClearForm();
                await LoadCoachesAsync();
            }
        }

        private async void btnUnlock_Click(
            object sender, RoutedEventArgs e)
        {
            if (!CheckAdmin()) return;

            if (_selectedCoach == null)
            {
                MessageBox.Show("Vui lòng chọn Coach.");
                return;
            }

            var result = await _service.SetLockedAsync(
                _selectedCoach.AccountId, false);

            MessageBox.Show(result.Message);

            if (result.Success)
            {
                ClearForm();
                await LoadCoachesAsync();
            }
        }

        private void btnClear_Click(
            object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            _selectedCoach = null;

            dgCoaches.SelectedItem = null;

            txtFullName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtPassword.Clear();
            txtSpecialization.Clear();
            txtWorkSchedule.Clear();

            lbDisciplines.UnselectAll();
        }
    }
}
