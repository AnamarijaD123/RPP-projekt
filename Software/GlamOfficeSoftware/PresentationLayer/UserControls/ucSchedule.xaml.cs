﻿using BusinessLogicLayer;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using EntityLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucSchedule.xaml
    /// </summary>
    public partial class ucSchedule : UserControl
    {
        public MainWindow Parent { get; set; }
        private ScheduleService _scheduleService = new ScheduleService();
        private EmployeeService employeeService = new EmployeeService();
        private List<DayDTO> _weekDays;
        private List<EmployeeDTO> _employees;

        private ucScheduleItem _selectedScheduleItem;
        private DailyScheduleDTO _selectedScheduleData;
        private bool isNextWeekActive = false;
        public ucSchedule()
        {
            InitializeComponent();
            _ = LoadData();
            _ = LoadEmployees();
            SetUserPermissions();
        }

        private void SetUserPermissions()
        {
            bool isAdmin = LoggedInEmployee.IsLoggedIn && LoggedInEmployee.LoggedEmployee.Role_idRole == 1;
            bool isNextWeek = isNextWeekActive;

            btnAdd.Visibility = (isAdmin && isNextWeek) ? Visibility.Visible : Visibility.Collapsed;
            btnEdit.Visibility = (isAdmin && isNextWeek) ? Visibility.Visible : Visibility.Collapsed;
            btnDelete.Visibility = (isAdmin && isNextWeek) ? Visibility.Visible : Visibility.Collapsed;

            cmbEmployees.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            btnApply.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }


        public async Task LoadData()
        {
            var scheduleService = new ScheduleService();

            DateTime startDate = isNextWeekActive ? GetNextMonday(DateTime.Today) : GetCurrentWeekMonday(DateTime.Today);

            _weekDays = (await scheduleService.GetOrCreateDaysForWeekAsync(startDate)).ToList();
            _employees = (await employeeService.GetAllEmployeesAsync()).ToList();

            int? filterEmployeeId = LoggedInEmployee.LoggedEmployee?.Role_idRole == 1 ? null : LoggedInEmployee.LoggedEmployee?.idEmployee;
            ClearScheduleDisplay();
            foreach (var day in _weekDays)
            {
                var schedules = (await scheduleService.GetSchedulesForDayAsync(day.Id))
                                .Where(s => filterEmployeeId == null || s.EmployeeId == filterEmployeeId)
                                .OrderBy(s => s.WorkStartTime)
                                .ToList();

                foreach (var schedule in schedules)
                {
                    var employee = _employees.FirstOrDefault(e => e.Id == schedule.EmployeeId);
                    if (employee != null)
                    {
                        AddScheduleItemToDay(day.Name, employee.Firstname, employee.Lastname,
                            schedule.WorkStartTime.HasValue ? schedule.WorkStartTime.Value.TimeOfDay : TimeSpan.Zero,
                            schedule.WorkEndTime.HasValue ? schedule.WorkEndTime.Value.TimeOfDay : TimeSpan.Zero,
                            schedule);
                    }
                }
            }

            btnNextWeek.Visibility = isNextWeekActive ? Visibility.Collapsed : Visibility.Visible;
            btnPrevWeek.Visibility = isNextWeekActive ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ClearScheduleDisplay()
        {
            wpMonday.Children.Clear();
            wpTuesday.Children.Clear();
            wpWednesday.Children.Clear();
            wpThursday.Children.Clear();
            wpFriday.Children.Clear();
            wpSaturday.Children.Clear();
            wpSunday.Children.Clear();
        }

        private void AddScheduleItemToDay(string dayName, string firstName, string lastName, TimeSpan startTime, TimeSpan endTime, DailyScheduleDTO scheduleData)
        {
            var scheduleItem = new ucScheduleItem(firstName, lastName, startTime, endTime, scheduleData, this);

            var dayMapping = new Dictionary<string, WrapPanel>
    {
        { "monday", wpMonday },
        { "tuesday", wpTuesday },
        { "wednesday", wpWednesday },
        { "thursday", wpThursday },
        { "friday", wpFriday },
        { "saturday", wpSaturday },
        { "sunday", wpSunday }
    };

            if (dayMapping.TryGetValue(dayName.ToLower(), out var panel))
            {
                scheduleItem.Margin = new Thickness(0, 5, 0, 0);
                panel.Children.Add(scheduleItem);
            }
        }

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            await LoadData();
            var sidebar = new ucAddScheduleSidebar(this,_weekDays, _employees);
            ccSidebar.Content = sidebar;
            ShowSidebar();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScheduleData == null)
            {
                MessageBox.Show("Please select a schedule entry to edit.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var sidebar = new ucEditScheduleSidebar(this,_weekDays, _employees, _selectedScheduleData);
            ccSidebar.Content = sidebar;
            ShowSidebar();
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedScheduleData == null)
            {
                MessageBox.Show("Choose schedule item to delete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete?", "Accept", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _scheduleService.DeleteDailyScheduleAsync(_selectedScheduleData.DayId, _selectedScheduleData.EmployeeId);

                    await LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error when deleting", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

       
        internal void ShowSidebar()
        {
            var slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                sidebarMenu.Visibility = Visibility.Visible;
                sidebarMenu.Margin = new Thickness(250, 0, 0, 0);

                var marginAnimation = new ThicknessAnimation
                {
                    From = new Thickness(250, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    Duration = new Duration(TimeSpan.FromSeconds(0.5)),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                sidebarMenu.BeginAnimation(MarginProperty, marginAnimation);
            }
        }

        internal void CloseSidebar()
        {
            var slideOutAnimation = FindResource("SlideOutAnimation") as Storyboard;
            var sidebarMenu = (FrameworkElement)ccSidebar.Content;

            if (sidebarMenu != null)
            {
                slideOutAnimation?.Begin(sidebarMenu);

                slideOutAnimation.Completed += (s, e) =>
                {
                    ccSidebar.Content = null;
                    sidebarMenu.Visibility = Visibility.Collapsed;
                };
            }
        }

        private async void btnNextWeek_Click(object sender, RoutedEventArgs e)
        {
            isNextWeekActive = true;
            await LoadData();
            SetUserPermissions();
        }

        private DateTime GetCurrentWeekMonday(DateTime date)
        {
            int daysSinceMonday = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysSinceMonday < 0) daysSinceMonday += 7;
            return date.AddDays(-daysSinceMonday);
        }

        private DateTime GetNextMonday(DateTime date)
        {
            return GetCurrentWeekMonday(date).AddDays(7);
        }

        private async void btnPrevWeek_Click(object sender, RoutedEventArgs e)
        {
            isNextWeekActive = false;
            await LoadData();
            SetUserPermissions();
        }


        public void SelectScheduleItem(ucScheduleItem selectedItem, DailyScheduleDTO scheduleData)
        {
            if (selectedItem == null || scheduleData == null) return;

            _selectedScheduleItem = selectedItem;
            _selectedScheduleData = scheduleData;

            foreach (var panel in new[] { wpMonday, wpTuesday, wpWednesday, wpThursday, wpFriday, wpSaturday, wpSunday })
            {
                foreach (var child in panel.Children)
                {
                    if (child is ucScheduleItem item)
                    {
                        item.Background = Brushes.White; 
                    }
                }
            }

            _selectedScheduleItem.Background = Brushes.LightGray;
        }

        public async Task LoadEmployees()
        {
            _employees = (await employeeService.GetAllEmployeesAsync())
                .Where(e => e.RoleName == "Regular user")
                .ToList();

            cmbEmployees.ItemsSource = _employees;
            cmbEmployees.DisplayMemberPath = "FullName";
            cmbEmployees.SelectedValuePath = "Id";
            cmbEmployees.SelectedIndex = 0;
        }

        private async void btnApply_Click_1(object sender, RoutedEventArgs e)
        {
            if (cmbEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedEmployeeId = (int)cmbEmployees.SelectedValue;
            await LoadFilteredSchedule(selectedEmployeeId);
        }

        public async Task LoadFilteredSchedule(int employeeId)
        {
            var scheduleService = new ScheduleService();
            DateTime startDate = isNextWeekActive ? GetNextMonday(DateTime.Today) : GetCurrentWeekMonday(DateTime.Today);

            _weekDays = (await scheduleService.GetOrCreateDaysForWeekAsync(startDate)).ToList();

            wpMonday.Children.Clear();
            wpTuesday.Children.Clear();
            wpWednesday.Children.Clear();
            wpThursday.Children.Clear();
            wpFriday.Children.Clear();
            wpSaturday.Children.Clear();
            wpSunday.Children.Clear();

            foreach (var day in _weekDays)
            {
                var schedules = (await scheduleService.GetSchedulesForDayAsync(day.Id))
                    .Where(s => s.EmployeeId == employeeId)
                    .OrderBy(s => s.WorkStartTime)
                    .ToList();

                foreach (var schedule in schedules)
                {
                    var employee = _employees.FirstOrDefault(e => e.Id == schedule.EmployeeId);
                    if (employee != null)
                    {
                        AddScheduleItemToDay(day.Name, employee.Firstname, employee.Lastname,
                            schedule.WorkStartTime.HasValue ? schedule.WorkStartTime.Value.TimeOfDay : TimeSpan.Zero,
                            schedule.WorkEndTime.HasValue ? schedule.WorkEndTime.Value.TimeOfDay : TimeSpan.Zero,
                            schedule);

                    }
                }
            }
        }

        private async void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEmployees.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedEmployeeId = (int)cmbEmployees.SelectedValue;
            await LoadFilteredSchedule(selectedEmployeeId);
        }

    }
}
