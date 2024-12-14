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
        if (!string.IsNullOrEmpty(search))
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
    public List<Employee> ApplySearch(List<Employee> employees)
    {
        string search = _searchTextBox.Text.ToLower();
        if (!string.IsNullOrEmpty(search))
        {
            employees = employees.Where(r => r.FullName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
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
                    return employees.OrderByDescending(e => e.IsArchive).ToList();
                default:
                    return employees.OrderByDescending(e => e.IdEmployee).ToList();
            }
        }
    }
}
