﻿using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Edits;
using RealtorsFirm_3cursEO.Model;
using RealtorsFirm_3cursEO.PagesAdmin;
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
using System.Windows.Shapes;

namespace RealtorsFirm_3cursEO
{
    /// <summary>
    /// Логика взаимодействия для MenuForAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {
        private Employee _employee;

        public MenuAdmin(Employee employee)
        {
            InitializeComponent();
            _employee = employee;

            ChoosePage(0);
        }

        private void ChoosePage(int numberPage)
        {
            switch (numberPage)
            {
                case 0:
                    ContentFrame.Navigate(new EmployeesAdmin(_employee));
                    break;
                case 1:
                    ContentFrame.Navigate(new ClientsAdmin(_employee));
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }
        private void ButtonEmployees_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(0);
        }

        private void ButtonClients_Click(object sender, RoutedEventArgs e)
        {
            ChoosePage(1);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = $"Меню для администратора. Сотрудник: {_employee.FullName}";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}