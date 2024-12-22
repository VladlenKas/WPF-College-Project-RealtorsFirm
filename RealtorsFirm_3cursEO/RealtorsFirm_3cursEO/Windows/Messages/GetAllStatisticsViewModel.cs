﻿using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorsFirm_3cursEO.Windows.Messages
{
    public class GetAllStatisticsViewModel(RealtorsFirmContext dbContext)
    {
        public List<Transaction> Transactions = new List<Transaction>(dbContext.Transactions);

        public double AverageCostTransaction => CalculateAverageCostTransaction(); // Работает
        public int AverageBonusesTransaction => CalculateAverageBonusesTransaction(); // Работает
        public int AverageCountPricesTransaction => CalculateAverageCountPricesTransaction(); // Работает
        public int AverageCountRepeatTransactionClients => CalculateAverageCountRepeatTransactionClients(); // Работает
        public int AverageCountEstatesClients => CalculateAverageCountEstatesClients(); // Проверить
        public int AverageCountBonusesClients => CalculateAverageCountBonusesClients(); // Проверить
        public double CurrencyPayment => CalculateCurrencyPayment();
        public double CurrencyBonuses => CalculateCurrencyBonuses();
        public int CountRegisteredClients => CalculateCountRegisteredClients();
        public int CountGuestClients => CalculateCountGuestClients();
        public int CountInProcessing => CalculateCountInProcessing();
        public int CountFinish => CalculateCountFinish();
        public int CountCancelled => CalculateCountCancelled();
        public int CountOnHold => CalculateCountOnHold();
        public int AmountClients => Transactions.Select(t => t.IdClient).Distinct().Count();
        public int AmountEstates => Transactions.Select(t => t.IdEstate).Distinct().Count();
        public int AmountPrices => Transactions.Sum(t => t.TransactionPriceRelations.Count);
        public int AmountTransactions => Transactions.Count;
        public decimal AmountEarned => Transactions.Sum(t => t.AmountDiscount);
        public int AmountBonuses => Transactions.Sum(t => t.AmountTotal - t.AmountDiscount);

        // Метод для расчета средней стоимости транзакции
        private double CalculateAverageCostTransaction() // Работает
        {
            return Transactions.Any() ? Transactions.Average(t => t.AmountTotal) : 0;
        }

        // Метод для расчета среднего количества бонусов по транзакциям
        private int CalculateAverageBonusesTransaction() // Работает
        {
            return Transactions.Any() ? (int)Transactions.Average(t => t.AmountTotal - t.AmountDiscount) : 0;
        }

        // Метод для расчета среднего количества услуг на чек
        private int CalculateAverageCountPricesTransaction() // Работает
        {
            return Transactions.Any() ? (int)Transactions.Average(t => t.TransactionPriceRelations.Count) : 0;
        }

        // Метод для расчета среднего количества повторных заказов клиентов
        private int CalculateAverageCountRepeatTransactionClients() // Работает
        {
            var repeatOrders = Transactions.GroupBy(t => t.IdClient)
                                           .Select(g => g.Count())
                                           .Where(count => count > 1)
                                           .ToList();

            return repeatOrders.Any() ? (int)repeatOrders.Average() : 0;
        }

        // Метод для расчета среднего количества недвижимости на клиента
        private int CalculateAverageCountEstatesClients() // Проверить
        {
            var clientEstateCounts = Transactions
               .GroupBy(t => t.IdClient) // Группируем по IdClient
               .Select(g => g.Select(t => t.IdEstate).Distinct().Count()) // Подсчитываем уникальные IdEstate для каждого клиента
               .ToList(); // Преобразуем в список

            return clientEstateCounts.Any() ? (int)Math.Round(clientEstateCounts.Average()) : 0;
        }

        // Метод для расчета среднего количества бонусов на клиента
        private int CalculateAverageCountBonusesClients() // Проверить
        {
            // Группируем транзакции по IdClient и суммируем бонусы для каждого клиента
            var clientsWithBonuses = Transactions
                .GroupBy(t => t.IdClient) // Группируем по IdClient
                .Select(g =>
                {
                    // Находим клиента по IdClient
                    var client = g.FirstOrDefault()?.IdEstateNavigation.IdClientNavigation; // Получаем навигационное свойство
                    return client?.Bonuses ?? 0; // Возвращаем бонусы клиента или 0, если клиент не найден
                })
                .ToList();

            // Вычисляем среднее количество бонусов на клиента
            return clientsWithBonuses.Any() ? (int)Math.Round(clientsWithBonuses.Average()) : 0; // Возвращаем среднее или 0, если нет клиентов
        }

        private double CalculateCurrencyPayment()
        {
            decimal totalPayments = AmountEarned; // Общая сумма заработка
            decimal bonusesPayments = AmountBonuses; // Общая сумма оплаченных бонусов
            decimal amountPayment = totalPayments + bonusesPayments; // Оплачено всего

            return (double)(totalPayments > 0 ? (totalPayments / amountPayment) * 100 : 0); // Процент
        }

        private double CalculateCurrencyBonuses()
        {
            decimal totalPayments = AmountEarned; // Общая сумма заработка
            decimal bonusesPayments = AmountBonuses; // Общая сумма оплаченных бонусов
            decimal amountPayment = totalPayments + bonusesPayments; // Оплачено всего

            return (double)(totalPayments > 0 ? (bonusesPayments / amountPayment) * 100 : 0); // Процент
        }

        private int CalculateCountRegisteredClients()
        {
            // Группируем транзакции по IdClient и находим зарегистрированных клиентов
            var registeredClients = Transactions
                .GroupBy(t => t.IdClient) // Группируем по IdClient
                .Where(g => g.FirstOrDefault()?.IdEstateNavigation.IdClientNavigation.IsRegistered == 1) // Фильтруем группы по статусу регистрации клиента
                .Count();

            var totalClients = Transactions
                .GroupBy(t => t.IdClient)
                .Count();

            return totalClients > 0 ? (registeredClients / totalClients) * 100 : 0;
        }

        private int CalculateCountGuestClients()
        {
            // Группируем транзакции по IdClient и находим незарегистрированных клиентов
            var notRegisteredClients = Transactions
                .GroupBy(t => t.IdClient) // Группируем по IdClient
                .Where(g => g.FirstOrDefault()?.IdEstateNavigation.IdClientNavigation.IsRegistered == 0) // Фильтруем группы по статусу регистрации клиента
                .Count();

            var totalClients = Transactions
                .GroupBy(t => t.IdClient)
                .Count();

            return totalClients > 0 ? (notRegisteredClients / totalClients) * 100 : 0;
        }

        private int CalculateCountInProcessing()
        {
            int statuses = Transactions.Count(t => t.IdStatus == 1);
            int totalStatuses = Transactions.Count();
            return statuses > 0 ? (statuses / totalStatuses) * 100 : 0;
        }

        private int CalculateCountFinish()
        {
            int statuses = Transactions.Count(t => t.IdStatus == 2);
            int totalStatuses = Transactions.Count();
            return statuses > 0 ? (statuses / totalStatuses) * 100 : 0;
        }


        private int CalculateCountCancelled()
        {
            int statuses = Transactions.Count(t => t.IdStatus == 3);
            int totalStatuses = Transactions.Count();
            return statuses > 0 ? (statuses / totalStatuses) * 100 : 0;
        }

        private int CalculateCountOnHold()
        {
            int statuses = Transactions.Count(t => t.IdStatus == 4);
            int totalStatuses = Transactions.Count();
            return statuses > 0 ? (statuses / totalStatuses) * 100 : 0;
        }

        public List<EstateTypeStatistics> CalculateEstateTypeStatistics()
        {
            // Получаем общее количество транзакций
            int totalTransactions = Transactions.Count;

            // Группируем транзакции по типу недвижимости и подсчитываем количество каждой группы
            var estateTypeCounts = Transactions
                .GroupBy(t => t.IdEstateNavigation.IdTypeNavigation.Name) // Предполагается, что у вас есть навигационное свойство IdTypeNavigation с названием типа
                .Select(g => new EstateTypeStatistics
                {
                    Name = g.Key,
                    Percent = totalTransactions > 0 ? (double)g.Count() / totalTransactions * 100 : 0 // Вычисляем процент
                })
                .ToList();

            return estateTypeCounts;
        }
    }

    public class EstateTypeStatistics
    {
        public string Name { get; set; } // Название типа недвижимости
        public double Percent { get; set; } // Процентное соотношение
    }
}

