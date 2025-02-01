﻿using BusinessLogicLayer.Exceptions;
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
    /// Interaction logic for ucEmployeeAdministration.xaml
    /// </summary>
    public partial class ucEmployeeAdministration : UserControl
    {
        private EmployeeService _employeeService = new EmployeeService();
        private ucAddNewEmployee _addNewEmployeeSidebar;
        public MainWindow Parent { get; set; }
        public ucEmployeeAdministration()
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
        }
        private async void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadFilters();
            LoadSortingList();
            ShowLoadingIndicator(true);
            await LoadEmployeesAsync();
            ShowLoadingIndicator(false);
        }

        private async Task LoadEmployeesAsync()
        {
            try
            {
                var employees = await Task.Run(() => _employeeService.GetAllEmployeesAsync());
                dgvEmployees.ItemsSource = employees;
            }
            catch (FailedToLoadClientsException ex)
            {
                throw new FailedToLoadClientsException("Error while loading employees.");
            }
        }

        private void LoadSortingList()
        {
            cmbSortingList.Items.Add("Name ascending");
            cmbSortingList.Items.Add("Name descending");
            cmbSortingList.Items.Add("Surname ascending");
            cmbSortingList.Items.Add("Surname descending");

            cmbSortingList.SelectedIndex = 0;
            cmbSortingList.IsDropDownOpen = false;
        }

        private void LoadFilters()
        {
            cmbFilters.Items.Add("Name");
            cmbFilters.Items.Add("Surname");
            cmbFilters.Items.Add("Key word");
            cmbFilters.Items.Add("Work position");
            cmbFilters.Items.Add("Role");

            cmbFilters.SelectedIndex = 0;
            cmbFilters.IsDropDownOpen = false;
        }

        private void LoadRolesAndPositions()
        {
            cmbFilterOptions.Items.Clear();

            if (cmbFilters.SelectedIndex == 3) 
            {
                cmbFilterOptions.Items.Add("Cosmetologist");
                cmbFilterOptions.Items.Add("Hairdresser");
                cmbFilterOptions.Items.Add("Dermatologist");
                cmbFilterOptions.Items.Add("Permanent Makeup Technician");
                cmbFilterOptions.Items.Add("Maderotherapy Specialist");
                cmbFilterOptions.Items.Add("Trainer");
                cmbFilterOptions.Items.Add("Polish Specialist");
                cmbFilterOptions.Items.Add("Nail Specialist");
                cmbFilterOptions.Items.Add("Permanent Depilation Specialist");
                cmbFilterOptions.Items.Add("Paste Specialist");
                cmbFilterOptions.Items.Add("Wax Specialist");
            }
            else if (cmbFilters.SelectedIndex == 4) 
            {
                cmbFilterOptions.Items.Add("Administrator");
                cmbFilterOptions.Items.Add("Regular User");
            }

            if (cmbFilterOptions.Items.Count > 0)
            {
                cmbFilterOptions.SelectedIndex = 0; 
            }
        }

        private void btnDropdownMenu_Click(object sender, RoutedEventArgs e)
        {
            cmbFilters.IsDropDownOpen = true;
        }

        private void cmbFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           
            if (cmbFilters.SelectedIndex < 0)
                return;

            switch (cmbFilters.SelectedIndex)
            {
                case 0: 
                    textSearch.Text = "Insert first name...";
                    textSearch.Visibility = Visibility.Visible;
                    txtSearch.Visibility = Visibility.Visible;
                    cmbFilterOptions.Visibility = Visibility.Collapsed;
                    break;
                case 1: 
                    textSearch.Text = "Insert surname...";
                    textSearch.Visibility = Visibility.Visible;
                    txtSearch.Visibility = Visibility.Visible;
                    cmbFilterOptions.Visibility = Visibility.Collapsed;
                    break;
                case 2: 
                    textSearch.Text = "Insert key word...";
                    textSearch.Visibility = Visibility.Visible;
                    txtSearch.Visibility = Visibility.Visible;
                    cmbFilterOptions.Visibility = Visibility.Collapsed;
                    break;
                case 3: 
                case 4: 
                    LoadRolesAndPositions();
                    textSearch.Text = "";
                    textSearch.Visibility = Visibility.Collapsed;
                    txtSearch.Visibility = Visibility.Collapsed;
                    cmbFilterOptions.Visibility = Visibility.Visible;
                    break;
            }
        }
        
        private void ShowLoadingIndicator(bool isLoading)
        {
            if (isLoading)
            {
                loadingIndicator.Visibility = Visibility.Visible;
                dgvEmployees.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadingIndicator.Visibility = Visibility.Collapsed;
                dgvEmployees.Visibility = Visibility.Visible;
            }
        }

        private void btnDropdownList_Click(object sender, RoutedEventArgs e)
        {
            cmbSortingList.IsDropDownOpen = true;
        }

        private void cmbSortingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgvEmployees.ItemsSource == null)
                return;

            var employees = dgvEmployees.ItemsSource.Cast<EmployeeDTO>().ToList(); 

            switch (cmbSortingList.SelectedIndex)
            {
                case 0: 
                    employees = employees.OrderBy(emp => emp.Firstname).ToList();
                    break;
                case 1:
                    employees = employees.OrderByDescending(emp => emp.Firstname).ToList();
                    break;
                case 2: 
                    employees = employees.OrderBy(emp => emp.Lastname).ToList();
                    break;
                case 3: 
                    employees = employees.OrderByDescending(emp => emp.Lastname).ToList();
                    break;
                default:
                    return;
            }

            dgvEmployees.ItemsSource = null;  
            dgvEmployees.ItemsSource = employees;
        }

        private void btnShowEmployeesDetails_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnAddNewEmployee_Click(object sender, RoutedEventArgs e)
        {
            
                if (ccSidebar.Content == null)
                {
                    _addNewEmployeeSidebar = new ucAddNewEmployee();
                    _addNewEmployeeSidebar.Parent = this.Parent;

                    ccSidebar.Content = _addNewEmployeeSidebar;
                    _addNewEmployeeSidebar.Visibility = Visibility.Visible;
                }
                else
                {
                    ccSidebar.Content = null;
                }

            
        }




        private void dgvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtSearch.Focus();
        }

        private async void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var pattern = txtSearch.Text.ToLower();

            if (!string.IsNullOrEmpty(pattern))
            {
                textSearch.Visibility = Visibility.Collapsed;
            }
            else
            {
                textSearch.Visibility = Visibility.Visible;
            }
            await SearchEmployeesAsync();
        }

        private async Task SearchEmployeesAsync()
        {
            string searchText = txtSearch.Text.Trim();
            int selectedFilter = cmbFilters.SelectedIndex;

            if (selectedFilter == 3 || selectedFilter == 4)
            {
                if (cmbFilterOptions.SelectedItem != null)
                {
                    searchText = cmbFilterOptions.SelectedItem.ToString();
                }
                else
                {
                    return; 
                }
            }

            if (string.IsNullOrEmpty(searchText))
            {
                ShowLoadingIndicator(true);
                await LoadEmployeesAsync();
                ShowLoadingIndicator(false);
                return;
            }

            try
            {
                ShowLoadingIndicator(true);
                IEnumerable<EmployeeDTO> employees = new List<EmployeeDTO>();

                switch (selectedFilter)
                {
                    case 0:
                        employees = await _employeeService.GetEmployeesByName(searchText);
                        break;
                    case 1:
                        employees = await _employeeService.GetEmployeesByLastName(searchText);
                        break;
                    case 2:
                        employees = await _employeeService.GetEmployeesByKeyPhrase(searchText);
                        break;
                    case 3:
                        employees = await _employeeService.GetEmployeesByWorkPosition(searchText);
                        break;
                    case 4: 
                        employees = await _employeeService.GetEmployeesByRole(searchText);
                        break;
                    default:
                        MessageBox.Show("Invalid filter selection.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }

                dgvEmployees.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while searching employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ShowLoadingIndicator(false);
            }
        }

        private void cmbFilters_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void cmbSortingList_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private async void cmbFilterOptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFilterOptions.SelectedItem == null)
                return;

            string selectedValue = cmbFilterOptions.SelectedItem.ToString();

            ShowLoadingIndicator(true);

            try
            {
                IEnumerable<EmployeeDTO> employees = new List<EmployeeDTO>();

                if (cmbFilters.SelectedIndex == 3) 
                {
                    employees = await _employeeService.GetEmployeesByWorkPosition(selectedValue);
                }
                else if (cmbFilters.SelectedIndex == 4) 
                {
                    employees = await  _employeeService.GetEmployeesByRole(selectedValue);
                }

                dgvEmployees.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while filtering employees: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ShowLoadingIndicator(false);
            }
        }
    }
}
