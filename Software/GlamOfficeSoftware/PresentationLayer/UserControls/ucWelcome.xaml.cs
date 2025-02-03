﻿using BusinessLogicLayer;
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
    /// Interaction logic for ucWelcome.xaml
    /// </summary>
    public partial class ucWelcome : UserControl
    {
        public ucWelcome()
        {
            InitializeComponent();

            txtUser.Text = $"Welcome {LoggedInEmployee.LoggedEmployee.Username} (Role: {LoggedInEmployee.LoggedEmployee.Role_idRole})";

        }
    }
}
