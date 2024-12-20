using Microsoft.EntityFrameworkCore;
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
        public static void AddEmployee(string Role, string Name, string Firstname, string Patronymic,
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
                    Patronymic = Patronymic,
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
        string Patronymic, DateOnly Birthday, string Phone, string Passport, string Email, string Password)
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
                newEmployee.Patronymic = Patronymic;
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

        // Архивирование
        public static void ArchiveClient(Client client)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                var employeeEdit = dbContext.Clients.Single(r => r.IdClient == client.IdClient);
                employeeEdit.IsArchive = 1;
                dbContext.SaveChanges();
            }
        }

        // Добавление
        public static void AddClient(string Name, string Firstname, string Patronymic,
        DateOnly Birthday, string Phone, string Passport, string Email, string Password)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                // Создаем новый объект Employee
                var newClient = new Client
                {
                    Name = Name,
                    Firstname = Firstname,
                    Patronymic = Patronymic,
                    Birthday = Birthday,
                    Phone = Phone,
                    Passport = Passport,
                    Email = Email,
                    Password = Password,
                };

                // Добавляем нового клиента в контекст
                dbContext.AllClients.Add(newClient);

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }
        }

        // Редактирование
        public static void EditClient(Client client, string Name, string Firstname,
        string Patronymic, DateOnly Birthday, string Phone, string Passport, string Email, string Password)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                // Находим пользователя по айди
                var newClient = dbContext.Clients.Single(r => r.IdClient == client.IdClient);

                // Обновляем данные
                newClient.Name = Name;
                newClient.Firstname = Firstname;
                newClient.Patronymic = Patronymic;
                newClient.Birthday = Birthday;
                newClient.Phone = Phone;
                newClient.Passport = Passport;
                newClient.Email = Email;
                newClient.Password = Password;

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region Услуги

        // Удаление
        public static void DeletePrice(Price price)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                var priceEdit = dbContext.Prices.Single(r => r.IdPrice == price.IdPrice);
                priceEdit.IsDeleted = 1;
                dbContext.SaveChanges();
            }
        }

        // Архивирование
        public static void ArchivePrice(Price price)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                var priceEdit = dbContext.Prices.Single(r => r.IdPrice == price.IdPrice);
                priceEdit.IsArchive = 1;
                dbContext.SaveChanges();
            }
        }

        // Добавление
        public static void AddPrice(string Name, int Cost)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                // Создаем новый объект Employee
                var newPrice = new Price
                {
                    Name = Name,
                    Cost = Cost,
                };

                // Добавляем нового сотрудника в контекст
                dbContext.AllPrices.Add(newPrice);

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }
        }

        // Редактирование
        public static void EditPrices(Price price, string Name, int Cost)
        {
            using (var dbContext = new RealtorsFirmContext())
            {
                // Находим услугу по айди
                var newEmployee = dbContext.Prices.Single(r => r.IdPrice == price.IdPrice);

                // Обновляем данные
                newEmployee.Name = Name;
                newEmployee.Cost = Cost;

                // Сохраняем изменения в базе данных
                dbContext.SaveChanges();
            }
        }
        #endregion

        #region Транзакция

        // Добавление
        public static void CreateTransaction(int IdEmployee, int IdClient, int IdEstate,
            int AmountTotal, int AmountDiscount, List<Price> prices, int AccrualBonuses, int DiscardBonuses, bool IsAccrualBonuses)
        {
            RealtorsFirmContext dbContext = new RealtorsFirmContext();

            // Создаем новый объект Transaction
            var newTransaction = new Transaction
            {
                IdEmployee = IdEmployee,
                IdClient = IdClient,
                IdEstate = IdEstate,
                Date = DateTime.Now,
                AmountTotal = AmountTotal,
                AmountDiscount = AmountDiscount
            };

            // Добавляем нового сотрудника в контекст
            dbContext.Transactions.Add(newTransaction);

            // Сохраняем изменения в базе данных
            dbContext.SaveChanges();

            // Обновляем бд
            dbContext = new();

            // Передаем id для табличной части
            int idTransaction = newTransaction.IdTransaction;
            // Создаем список для хранения отношений цен
            foreach (var price in prices)
            {
                var priceOutDb = dbContext.Prices.Single(r => r.IdPrice == price.IdPrice);
                TransactionPriceRelation relation = new TransactionPriceRelation()
                {
                    IdTransaction = idTransaction,
                    IdPrice = priceOutDb.IdPrice
                };
                dbContext.TransactionPriceRelations.Add(relation);
            }

            // Сохраняем изменения в базе данных
            dbContext.SaveChanges();

            // Начисляем или списываем бонусы у клиента
            var client = dbContext.Clients.Single(r => r.IdClient == IdClient);
            if (IsAccrualBonuses)
            {
                client.Bonuses += AccrualBonuses; // Начисляем бонусы
            }
            else
            {
                client.Bonuses -= DiscardBonuses; // Списываем бонусы
            }

            //  Обновляем и сохраняем изменения в базе данных
            dbContext.Update(client);
            dbContext.SaveChanges();
        }

        #endregion
    }
}
