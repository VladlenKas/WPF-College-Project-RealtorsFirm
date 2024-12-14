using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RealtorsFirm_3cursEO.Classes
{
    public static class UploadDataFilter
    {
        private static RealtorsFirmContext dbContext = new();

        private readonly static List<object> _filterList = new List<object>
        {
            "Без фильтрации",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };
        private readonly static List<object> _sorterList = new List<object>
        {
            "По дате добавления",
            new Separator { Margin = new Thickness(0, 5, 0, 5), Width = 150 }
        };

        #region Сотрудники

        public static List<object> FilterEmployees()
        {
            var filterList = new List<object>(_filterList);
            foreach (var role in dbContext.RoleEmployees)
            {
                filterList.Add(role.Name);
            }
            return filterList;
        }

        public static List<object> SorterEmployees()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По имени",
                "По фамилии",
                "По отчеству",
                "По дате рождения",
                "По паспорту",
                "По номеру телефона",
                "По электронной почте",
                "По паролю",
                "По должности",
                "По статусу удаления"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }
        #endregion

        #region Клиенты
        public static List<object> SorterClients()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По имени",
                "По фамилии",
                "По отчеству",
                "По дате рождения",
                "По паспорту",
                "По номеру телефона",
                "По электронной почте",
                "По статусу удаления"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }
        #endregion

        #region Услуги
        public static List<object> SorterPrices()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По наименованию",
                "По цене",
                "По статусу удаления"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }
        #endregion

        #region Недвижимость

        public static List<object> FilterEstates()
        {
            var filterList = new List<object>(_filterList);
            foreach (var role in dbContext.TypeEstates)
            {
                filterList.Add(role.Name);
            }
            return filterList;
        }

        public static List<object> SorterEstates()
        {
            var sorterList = new List<object>(_sorterList);
            var strings = new List<object>
            {
                "По ФИО клиента",
                "По типу недвижмости",
                "По адресу",
                "По площади",
                "По кол-ву комнат",
                "По цене",
                "По статусу удаления"
            };
            sorterList.AddRange(strings);
            return sorterList;
        }
        #endregion
    }
}
