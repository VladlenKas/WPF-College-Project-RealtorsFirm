using Microsoft.EntityFrameworkCore;
using RealtorsFirm_3cursEO.Classes;
using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtorsFirm_3cursEO.Pages
{   
    public class GetAllStatisticsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Transaction> _transactions;
        private DataFilterTransactions DataFilterTransactions;

        public event PropertyChangedEventHandler? PropertyChanged;

        public GetAllStatisticsViewModel(DataFilterTransactions DataFilterTransactions)
        {
            this.DataFilterTransactions = DataFilterTransactions;
            UpdateData(); 
        }

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Метод для обновления данных
        public void UpdateData()
        {
            RealtorsFirmContext dbContext = new RealtorsFirmContext();
            dbContext.Transactions
                .Include(r => r.IdEmployeeNavigation)
                .Include(r => r.IdStatusNavigation)
                .Include(r => r.IdEstateNavigation)
                .ThenInclude(e => e.IdClientNavigation)
                .Load();

            var transactionsList = dbContext.Transactions.ToList();

            transactionsList = DataFilterTransactions.ApplyFilter(transactionsList);
            transactionsList = DataFilterTransactions.ApplySorter(transactionsList);
            transactionsList = DataFilterTransactions.ApplySearch(transactionsList);

            Transactions.Clear(); // Очищаем текущую коллекцию

            foreach (var transaction in transactionsList)
            {
                Transactions.Add(transaction); // Добавляем новые элементы
            }
        }
    }
}
