﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using static MaterialDesignThemes.Wpf.Theme.ToolBar;

namespace RealtorsFirm_3cursEO.UserControls.TextBoxes
{
    /// <summary>
    /// Логика взаимодействия для AutoCompleteControl.xaml
    /// </summary>
    public partial class AutoCompleteControl : UserControl
    {
        // Зависимое свойство для источника элементов (ItemsSource)
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(AutoCompleteControl), new PropertyMetadata(null, OnItemsSourceChanged));

        /// <summary>
        /// Источник данных для автозаполнителя.
        /// </summary>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Зависимое свойство для пути к отображаемому свойству (DisplayMemberPath)
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(AutoCompleteControl), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Имя свойства, которое будет отображаться в списке.
        /// </summary>
        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }
        private string _displayMemberPath;

        /// <summary>
        /// Текст автозаполнителя.
        /// </summary>
        public string Text
        {
            set
            {
                InputTextBox.Text = value;
                ItemsListBox.SelectedItem = null;
                SelectedItem = null;
                ItemsListBox.Visibility = Visibility.Collapsed;
            }
        }

        // Выбранный элемент из списка
        private object _selectedItem;
        public object SelectedItem 
        {
            get { return _selectedItem; } 
            set
            {
                _selectedItem = value;
                ItemsListBox.SelectedItem = _selectedItem;
            }
        }

        /// <summary>
        /// Конструктор элемента управления.
        /// </summary>
        public AutoCompleteControl()
        {
            InitializeComponent();
            Loaded += AutoCompleteControl_Loaded; // Подписка на событие загрузки
        }

        /// <summary>
        /// Обработчик события загрузки элемента управления.
        /// </summary>
        private void AutoCompleteControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateListBox(); // Обновляем список при загрузке
        }

        /// <summary>
        /// Обработчик изменения источника данных.
        /// </summary>
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as AutoCompleteControl;
            control?.UpdateListBox(); // Обновляем список при изменении источника данных
        }

        /// <summary>
        /// Обработчик изменения текста в TextBox.
        /// </summary>
        private void InputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SelectedItem != null)
            {
                SelectedItem = null;
                ItemsListBox.SelectedItem = null;
                ItemSelected?.Invoke(null); 
            }

            UpdateListBox(); // Обновляем список при изменении текста
            ItemsListBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Обновляет элементы в списке на основе введенного текста.
        /// </summary>
        private void UpdateListBox()
        {
            if (_displayMemberPath == null)
            {
                _displayMemberPath = DisplayMemberPath;
            }
            else
            {
                DisplayMemberPath = _displayMemberPath;
            }

            if (ItemsSource == null)
            {
                ItemsListBox.ItemsSource = null; // Или можно установить пустую коллекцию
                return;
            }

            var filter = InputTextBox.Text.ToLower(); // Получаем текст в нижнем регистре

            if (string.IsNullOrWhiteSpace(filter))
            {
                // Если текст пустой, показываем все элементы
                ItemsListBox.ItemsSource = ItemsSource.Cast<object>().ToList();
            }
            else
            {
                // Фильтруем элементы на основе введенного текста
                ItemsListBox.ItemsSource = ItemsSource.Cast<object>()
                    .Where(item => FilterItem(item, filter)).ToList();

                if (ItemsListBox.Items.Count == 0)
                {
                    DisplayMemberPath = "Message";
                    // Если ничего не найдено, можно добавить элемент "Ничего не найдено"
                    var noDataItem = new[] { new { Message = "Ничего не найдено" } };
                    ItemsListBox.ItemsSource = noDataItem;
                }
                else if (ItemsListBox.ItemsSource == null)
                {
                    DisplayMemberPath = "Message";
                    // Если коллекции нет, можно добавить элемент "Нет элементов для выбора"
                    var noDataItem = new[] { new { Message = "Нет элементов для выбора" } };
                    ItemsListBox.ItemsSource = noDataItem;
                }
            }
        }

        /// <summary>
        /// Фильтрует элемент на основе введенного текста.
        /// </summary>
        private bool FilterItem(object item, string filter)
        {
            if (item == null) return false;

            if (DisplayMemberPath == "None") // Проверка на специальное значение
            {
                return false; // Не фильтруем, если DisplayMemberPath отключен
            }

            PropertyInfo propertyInfo = item.GetType().GetProperty(DisplayMemberPath);
            if (propertyInfo != null)
            {
                var value = propertyInfo.GetValue(item)?.ToString().ToLower();
                return value != null && value.Contains(filter); // Проверяем наличие фильтра в значении свойства
            }

            return false;
        }

        /// <summary>
        /// Обработчик нажатия кнопки для отображения/скрытия списка.
        /// </summary>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ItemsListBox.Visibility = ItemsListBox.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; // Переключаем видимость списка
        }

        /// <summary>
        /// Проверка на анонимный тип данных
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var type = obj.GetType();
            return type.IsGenericType && type.Name.StartsWith("<>f__AnonymousType");
        }

        /// <summary>
        /// Обработчик изменения выбора элемента в списке.
        /// </summary>
        private void ItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ItemsListBox.SelectedItem is object selectedItem)
            {
                if (IsAnonymousType(selectedItem))
                {
                    ItemsListBox.SelectedItem = null;
                    return;
                }

                SelectedItem = selectedItem; // Сохраняем выбранный элемент
                PropertyInfo propertyInfo = selectedItem.GetType().GetProperty(DisplayMemberPath);

                InputTextBox.TextChanged -= InputTextBox_TextChanged;
                InputTextBox.Text = propertyInfo?.GetValue(selectedItem)?.ToString(); // Устанавливаем текст в TextBox
                InputTextBox.TextChanged += InputTextBox_TextChanged;

                // Вызываем событие уведомления о выборе элемента
                ItemSelected?.Invoke(selectedItem);

            }
            ItemsListBox.Visibility = Visibility.Collapsed; // Скрываем список после выбора
        }

        /// <summary>
        /// Обработчик получения фокуса для поисковой строки.
        /// </summary>
        private void InputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ItemsListBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Событие, вызываемое при выборе элемента из списка.
        /// </summary>
        public event Action<object> ItemSelected;
    }
}
