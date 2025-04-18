﻿using EntityLayer.DTOs;
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
    /// Interaction logic for ucScheduleItem.xaml
    /// </summary>
    public partial class ucScheduleItem : UserControl
    {
        private ucSchedule _parent;

        public ucScheduleItem(string firstName, string lastName, TimeSpan startTime, TimeSpan endTime, DailyScheduleDTO scheduleData, ucSchedule parent)
        {
            InitializeComponent();
            txtEmployeeName.Text = $"{firstName} {lastName}";
            txtTimeRange.Text = $"{startTime:hh\\:mm} - {endTime:hh\\:mm}";

            this.Tag = scheduleData;
            _parent = parent;

            this.MouseLeftButtonDown += ScheduleItem_Click;
        }

        private void ScheduleItem_Click(object sender, MouseButtonEventArgs e)
        {
            _parent.SelectScheduleItem(this, (DailyScheduleDTO)this.Tag);
        }

    }
}
