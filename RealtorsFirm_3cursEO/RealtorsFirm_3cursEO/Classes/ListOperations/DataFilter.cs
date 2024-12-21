using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RealtorsFirm_3cursEO.Classes;

/// <summary>
/// Фильтрация данных для сотрудников
/// </summary>
public class DataFilterEmployees
{
    private TextBox _searchTextBox;
    private ComboBox _filterComboBox;
    private ComboBox _sorterComboBox;
    private bool _ascending;

    public DataFilterEmployees(TextBox searchTextBox, ComboBox filterComboBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox)
    {
        _searchTextBox = searchTextBox;
        _filterComboBox = filterComboBox;
        _sorterComboBox = sorterComboBox;

        if (ascendingCheckBox != null)
            _ascending = (bool)ascendingCheckBox.IsChecked;
        else
            _ascending = false;
    }

    // Поиск 
    public List<Employee> ApplySearch(List<Employee> employees)
    {
        string search = _searchTextBox.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            employees = employees.Where(r => r.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return employees;
    }

    // Фильтрафия
    public List<Employee> ApplyFilter(List<Employee> employees)
    {
        if (_filterComboBox.SelectedIndex != 0)
        {
            string role = _filterComboBox.SelectedValue.ToString();
            employees = employees.Where(u => u.IdRoleNavigation.Name == role).ToList();
        }
        return employees;
    }

    // Сортировка
    public List<Employee> ApplySorter(List<Employee> employees)
    {
        int sortIndex = _sorterComboBox.SelectedIndex;

        if (!_ascending)
        {
            switch (sortIndex)
            {
                case 2:
                    return employees.OrderBy(e => e.Name).ToList();
                case 3:
                    return employees.OrderBy(e => e.Firstname).ToList();
                case 4:
                    return employees.OrderBy(e => e.Patronymic).ToList();
                case 5:
                    return employees.OrderBy(e => e.Birthday).ToList();
                case 6:
                    return employees.OrderBy(e => e.Passport).ToList();
                case 7:
                    return employees.OrderBy(e => e.Phone).ToList();
                case 8:
                    return employees.OrderBy(e => e.Email).ToList();
                case 9:
                    return employees.OrderBy(e => e.Password).ToList();
                case 10:
                    return employees.OrderBy(e => e.IdRoleNavigation.Name).ToList();
                case 11:
                    return employees.OrderBy(e => e.IsArchive).ToList();
                default:
                    return employees.OrderBy(e => e.IdEmployee).ToList();
            }
        }
        else
        {
            switch (sortIndex)
            {
                case 2:
                    return employees.OrderByDescending(e => e.Name).ToList();
                case 3:
                    return employees.OrderByDescending(e => e.Firstname).ToList();
                case 4:
                    return employees.OrderByDescending(e => e.Patronymic).ToList();
                case 5:
                    return employees.OrderByDescending(e => e.Birthday).ToList();
                case 6:
                    return employees.OrderByDescending(e => e.Passport).ToList();
                case 7:
                    return employees.OrderByDescending(e => e.Phone).ToList();
                case 8:
                    return employees.OrderByDescending(e => e.Email).ToList();
                case 9:
                    return employees.OrderByDescending(e => e.Password).ToList();
                case 10:
                    return employees.OrderByDescending(e => e.IdRoleNavigation.Name).ToList();
                case 11:
                    return employees.OrderByDescending(e => e.IsArchive).ToList();
                default:
                    return employees.OrderByDescending(e => e.IdEmployee).ToList();
            }
        }
    }
}

/// <summary>
/// Фильтрация данных для клиента
/// </summary>
public class DataFilterClients
{
    private TextBox _searchTextBox;
    private ComboBox _sorterComboBox;
    private bool _ascending;

    public DataFilterClients(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox)
    {
        _searchTextBox = searchTextBox;
        _sorterComboBox = sorterComboBox;

        if (ascendingCheckBox != null)
            _ascending = (bool)ascendingCheckBox.IsChecked;
        else
            _ascending = false;
    }

    // Поиск 
    public List<Client> ApplySearch(List<Client> clients)
    {
        string search = _searchTextBox.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            clients = clients.Where(r => r.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return clients;
    }

    // Сортировка
    public List<Client> ApplySorter(List<Client> clients)
    {
        int sortIndex = _sorterComboBox.SelectedIndex;

        if (!_ascending)
        {
            switch (sortIndex)
            {
                case 2:
                    return clients.OrderBy(e => e.Name).ToList();
                case 3:
                    return clients.OrderBy(e => e.Firstname).ToList();
                case 4:
                    return clients.OrderBy(e => e.Patronymic).ToList();
                case 5:
                    return clients.OrderBy(e => e.Birthday).ToList();
                case 6:
                    return clients.OrderBy(e => e.Passport).ToList();
                case 7:
                    return clients.OrderBy(e => e.Phone).ToList();
                case 8:
                    return clients.OrderBy(e => e.Email).ToList();
                case 9:
                    return clients.OrderBy(e => e.IsArchive).ToList();
                default:
                    return clients.OrderBy(e => e.IdClient).ToList();
            }
        }
        else
        {
            switch (sortIndex)
            {
                case 2:
                    return clients.OrderByDescending(e => e.Name).ToList();
                case 3:
                    return clients.OrderByDescending(e => e.Firstname).ToList();
                case 4:
                    return clients.OrderByDescending(e => e.Patronymic).ToList();
                case 5:
                    return clients.OrderByDescending(e => e.Birthday).ToList();
                case 6:
                    return clients.OrderByDescending(e => e.Passport).ToList();
                case 7:
                    return clients.OrderByDescending(e => e.Phone).ToList();
                case 8:
                    return clients.OrderByDescending(e => e.Email).ToList();
                case 9:
                    return clients.OrderByDescending(e => e.IsArchive).ToList();
                default:
                    return clients.OrderByDescending(e => e.IdClient).ToList();
            }
        }
    }
}

/// <summary>
/// Фильтрация данных для недвижимости
/// </summary>
public class DataFilterEstates
{
    private ComboBox _filterComboBox;
    private TextBox _searchTextBox;
    private ComboBox _sorterComboBox;
    private bool _ascending;

    public DataFilterEstates(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox, ComboBox filterComboBox)
    {
        _searchTextBox = searchTextBox;
        _sorterComboBox = sorterComboBox;
        _filterComboBox = filterComboBox;

        if (ascendingCheckBox != null)
            _ascending = (bool)ascendingCheckBox.IsChecked;
        else
            _ascending = false;
    }

    // Фильтрафия
    public List<Estate> ApplyFilter(List<Estate> estates)
    {
        if (_filterComboBox.SelectedIndex != 0 && _filterComboBox.SelectedValue != null)
        {
            string type = _filterComboBox.SelectedValue.ToString();
            estates = estates.Where(u => u.IdTypeNavigation.Name == type).ToList();
        }
        return estates;
    }

    // Поиск 
    public List<Estate> ApplySearch(List<Estate> estates)
    {
        string search = _searchTextBox.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            estates = estates.Where(r => r.IdClientNavigation.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return estates;
    }

    // Сортировка
    public List<Estate> ApplySorter(List<Estate> estates)
    {
        int sortIndex = _sorterComboBox.SelectedIndex;

        if (!_ascending)
        {
            switch (sortIndex)
            {
                case 2:
                    return estates.OrderBy(e => e.IdClientNavigation.FullName).ToList();
                case 3:
                    return estates.OrderBy(e => e.IdTypeNavigation.Name).ToList();
                case 4:
                    return estates.OrderBy(e => e.Address).ToList();
                case 5:
                    return estates.OrderBy(e => e.Area).ToList();
                case 6:
                    return estates.OrderBy(e => e.NumberRooms).ToList();
                case 7:
                    return estates.OrderBy(e => e.Cost).ToList();
                case 8:
                    return estates.OrderBy(e => e.IsArchive).ToList();
                default:
                    return estates.OrderBy(e => e.IdEstate).ToList();
            }
        }
        else
        {
            switch (sortIndex)
            {
                case 2:
                    return estates.OrderByDescending(e => e.IdClientNavigation.FullName).ToList();
                case 3:
                    return estates.OrderByDescending(e => e.IdTypeNavigation.Name).ToList();
                case 4:
                    return estates.OrderByDescending(e => e.Address).ToList();
                case 5:
                    return estates.OrderByDescending(e => e.Area).ToList();
                case 6:
                    return estates.OrderByDescending(e => e.NumberRooms).ToList();
                case 7:
                    return estates.OrderByDescending(e => e.Cost).ToList();
                case 8:
                    return estates.OrderByDescending(e => e.IsArchive).ToList();
                default:
                    return estates.OrderByDescending(e => e.IdEstate).ToList();
            }
        }
    }
}

/// <summary>
/// Фильтрация данных для клиента
/// </summary>
public class DataFilterPrices
{
    private TextBox _searchTextBox;
    private ComboBox _sorterComboBox;
    private bool _ascending;

    public DataFilterPrices(TextBox searchTextBox, ComboBox sorterComboBox, CheckBox ascendingCheckBox)
    {
        _searchTextBox = searchTextBox;
        _sorterComboBox = sorterComboBox;

        if (ascendingCheckBox != null)
            _ascending = (bool)ascendingCheckBox.IsChecked;
        else
            _ascending = false;
    }

    // Поиск 
    public List<Price> ApplySearch(List<Price> prices)
    {
        string search = _searchTextBox.Text.ToLower();
        if (!string.IsNullOrWhiteSpace(search))
        {
            prices = prices.Where(r => r.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        return prices;
    }

    // Сортировка
    public List<Price> ApplySorter(List<Price> prices)
    {
        int sortIndex = _sorterComboBox.SelectedIndex;

        if (!_ascending)
        {
            switch (sortIndex)
            {
                case 2:
                    return prices.OrderBy(e => e.Name).ToList();
                case 3:
                    return prices.OrderBy(e => e.Cost).ToList();
                case 4:
                    return prices.OrderBy(e => e.IsArchive).ToList();
                default:
                    return prices.OrderBy(e => e.IdPrice).ToList();
            }
        }
        else
        {
            switch (sortIndex)
            {
                case 2:
                    return prices.OrderByDescending(e => e.Name).ToList();
                case 3:
                    return prices.OrderByDescending(e => e.Cost).ToList();
                case 4:
                    return prices.OrderByDescending(e => e.IsArchive).ToList();
                default:
                    return prices.OrderByDescending(e => e.IdPrice).ToList();
            }
        }
    }
}

