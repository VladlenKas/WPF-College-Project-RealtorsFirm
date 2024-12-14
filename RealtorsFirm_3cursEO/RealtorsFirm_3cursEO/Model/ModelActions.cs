﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RealtorsFirm_3cursEO.Model
{
    public static class ModelActions
    {
        #region Сотрудники

        // Удаление
        public static void DeleteEmployee(Employee employee)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                var employeeEdit = dbContext.Employees.Single(r => r.IdEmployee == employee.IdEmployee);
                employeeEdit.IsDeleted = 1;
                dbContext.SaveChanges();
            }
        }

        // Архивирование
        public static void ArchiveEmployee(Employee employee)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                var employeeEdit = dbContext.Employees.Single(r => r.IdEmployee == employee.IdEmployee);
                employeeEdit.IsArchive = 1;
                dbContext.SaveChanges();
            }
        }

        // Добавление
        public static void AddEmployee(string Role, string Name, string Firstname,
        DateOnly Birthday, string Phone, string Passport, string Email, string Password)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                // Находим айди роли по названию
                int idRole = dbContext.RoleEmployees.Single(r => r.Name == Role).IdRole;

                // Создаем новый объект Employee
                var newEmployee = new Employee
                {
                    IdRole = idRole,
                    Name = Name,
                    Firstname = Firstname,
                    Birthday = Birthday,
                    Phone = Phone,
                    Passport = Passport,
                    Email = Email,
                    Password = Password,
                };

                // Добавляем нового сотрудника в контекст
                dbContext.AllEmployees.Add(newEmployee);

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }
        }

        // Редактирование
        public static void EditEmployee(Employee employee, string Role, string Name, string Firstname,
        DateOnly Birthday, string Phone, string Passport, string Email, string Password)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                // Находим айди роли по названию
                int idRole = dbContext.RoleEmployees.Single(r => r.Name == Role).IdRole;
                // Находим пользователя по айди
                var newEmployee = dbContext.Employees.Single(r => r.IdEmployee == employee.IdEmployee);

                // Обновляем данные
                newEmployee.Name = Name;
                newEmployee.Firstname = Firstname;
                newEmployee.Birthday = Birthday;
                newEmployee.Phone = Phone;
                newEmployee.Passport = Passport;
                newEmployee.Email = Email;
                newEmployee.Password = Password; 
                newEmployee.IdRole = idRole; 

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region Клиенты

        // Удаление
        public static void DeleteClient(Client client)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                var employeeEdit = dbContext.Clients.Single(r => r.IdClient == client.IdClient);
                employeeEdit.IsDeleted = 1;
                dbContext.SaveChanges();
            }
        }

        // Добавление

        // Редактирование

        #endregion
    }
}