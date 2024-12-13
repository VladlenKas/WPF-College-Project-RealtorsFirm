using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RealtorsFirm_3cursEO.Classes.DataOperations
{
    public static class DataLimitators
    {
        // Создание/редактирование сотрдуника
        public static bool ValidationEmployee(Employee employee, string Role, string Name, string Firstname,
            DateOnly Birthday, string Phone, string Passport, string Email, string Password)
        {
            using (var context = new RealtorsFirmContext())
            {
                // Создаем лист для исключений
                List<string> errorsList = new List<string>();

                // Проверка на пустые поля
                if (new[] { Role, Name, Firstname, Phone, Passport, Email, Password }.Any(string.IsNullOrWhiteSpace))
                {
                    errorsList.Add("Заполните все обязательные поля.");
                }
                // Все остальные проверки
                else
                {
                    // Проверки для даты
                    bool ageIsValid = DataLimitatorsMethods.LimitatorAge(Birthday);
                    // Проверка для емайла
                    bool emailIsValid = DataLimitatorsMethods.LimitatorEmail(Email);

                    // Проверка на возраст (на валидную дату)
                    if (Birthday.Equals(DateOnly.MinValue))
                    {
                        errorsList.Add("Укажите корректный формат для даты.");
                    }

                    // Проверка на возраст (на допустимый возраст)
                    else if (!ageIsValid)
                    {
                        errorsList.Add("Возраст должен быть от 18 до 85 лет.");
                    }

                    // Проверка на номер телефона (на мин. длину)
                    else if (Phone.Length < 11)
                    {
                        errorsList.Add("Номер телефона должен содержать 11 цифр.");
                    }

                    // Проверка на паспорт (на мин. длину)
                    if (Passport.Length < 10)
                    {
                        errorsList.Add("Паспорт должен содержать 10 цифр.");
                    }

                    // Проверка на верный формат эл почты
                    if (!emailIsValid)
                    {
                        errorsList.Add("Укажите корректный формат для электронной почты.");
                    }

                    // При редактировании
                    if (employee != null)
                    {
                        // Проверка на номер телефона (на повторение)
                        if (context.Employees.Any(r => r.Phone == Phone && r.IdEmployee != employee.IdEmployee))
                        {
                            errorsList.Add("Введенный номер телефона уже существует. Используйте другой.");
                        }

                        // Проверка на паспорт (на повторение)
                        else if (context.Employees.Any(r => r.Passport == Passport && r.IdEmployee != employee.IdEmployee))
                        {
                            errorsList.Add("Введенный паспорт уже существует. Используйте другой.");
                        }

                        // Проверка на электронную почту (на повторение)
                        else if (context.Employees.Any(r => r.Email == Email && r.IdEmployee != employee.IdEmployee))
                        {
                            errorsList.Add("Введенный адрес электронной почты уже существует. Используйте другой.");
                        }
                    }
                    // При добавлении
                    else
                    {
                        // Проверка на номер телефона (на повторение)
                        if (context.Employees.Any(r => r.Phone == Phone))
                        {
                            errorsList.Add("Введенный номер телефона уже существует. Используйте другой.");
                        }

                        // Проверка на паспорт (на повторение)
                        else if (context.Employees.Any(r => r.Passport == Passport))
                        {
                            errorsList.Add("Введенный паспорт уже существует. Используйте другой.");
                        }

                        // Проверка на электронную почту (на повторение)
                        else if (context.Employees.Any(r => r.Email == Email))
                        {
                            errorsList.Add("Введенный адрес электронной почты уже существует. Используйте другой.");
                        }
                    }
                }

                if (errorsList.Count != 0)
                {
                    string error = errorsList[0];

                    MessageBox.Show($"{error}", "Ошибка.", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
