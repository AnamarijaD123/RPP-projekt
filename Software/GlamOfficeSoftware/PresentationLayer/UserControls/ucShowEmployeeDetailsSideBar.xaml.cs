﻿using BusinessLogicLayer.Services;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for ucShowEmployeeDetailsSideBar.xaml
    /// </summary>
    public partial class ucShowEmployeeDetailsSideBar : UserControl
    {
        public EmployeeDTO SelectedEmployee { get; set; }

        public ucEmployeeAdministration Parent { get; set; }

        private EmployeeService _employeeService;
        public ucShowEmployeeDetailsSideBar(EmployeeDTO employee)
        {
            InitializeComponent();
            _employeeService = new EmployeeService();
            SelectedEmployee = employee;
            LoadEmployeeDetailsAsync();
        }

        private async Task LoadEmployeeDetailsAsync()
        {
            if (SelectedEmployee != null)
            {
                txtPIN.Text = SelectedEmployee.PIN;
                txtFirstname.Text = SelectedEmployee.Firstname;
                txtLastname.Text = SelectedEmployee.Lastname;
                txtEmail.Text = SelectedEmployee.Email;
                txtUsername.Text = SelectedEmployee.Username;
                txtPassword.Text = SelectedEmployee.Password;
                txtSalt.Text = SelectedEmployee.Salt;
                txtPhoneNumber.Text = SelectedEmployee.PhoneNumber;
                txtRole.Text = SelectedEmployee.RoleName;
                txtWorkPosition.Text = SelectedEmployee.WorkPositionName;


            }
        }
        

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCloseSidebar_Click(object sender, RoutedEventArgs e)
        {
            Parent.CloseSideBarMenu();
        }
    }
}
