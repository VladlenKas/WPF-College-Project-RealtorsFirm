using RealtorsFirm_3cursEO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace RealtorsFirm_3cursEO.Classes.DataOperations
{
    public static class DataLimitators
    {
        #region Сотрудники
        public static bool LimitatorEmployee(Employee? employee, string Role, string Name, string Firstname,
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
                    // Проверки для имени
                    bool nameIsValid = LimitatorName(Name);
                    // Проверки для даты
                    bool ageIsValid = LimitatorAge(Birthday);
                    // Проверка для емайла
                    bool emailIsValid = LimitatorEmail(Email);

                    // Проверка на имя
                    if (!nameIsValid)
                    {
                        errorsList.Add("Имя, содержащее тире, должно  быть формата \"имя-имя\".");
                    }
                    // Проверка на возраст (на валидную дату)
                    else if (Birthday.Equals(DateOnly.MinValue))
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

        #endregion

        #region Клиенты
        public static bool LimitatorClient(Client? client, string Name, string Firstname,
            DateOnly Birthday, string Phone, string Passport, string Email, string Password)
        {
            using (var context = new RealtorsFirmContext())
            {
                // Создаем лист для исключений
                List<string> errorsList = new List<string>();

                // Проверка на пустые поля
                if (new[] { Name, Firstname, Phone, Passport, Email, Password }.Any(string.IsNullOrWhiteSpace))
                {
                    errorsList.Add("Заполните все обязательные поля.");
                }
                // Все остальные проверки
                else
                {
                    // Проверки для имени
                    bool nameIsValid = LimitatorName(Name);
                    // Проверки для даты
                    bool ageIsValid = LimitatorAge(Birthday);
                    // Проверка для емайла
                    bool emailIsValid = LimitatorEmail(Email);

                    // Проверка на имя
                    if (!nameIsValid)
                    {
                        errorsList.Add("Имя, содержащее тире, должно  быть формата \"имя-имя\".");
                    }
                    // Проверка на возраст (на валидную дату)
                    else if (Birthday.Equals(DateOnly.MinValue))
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
                    if (client != null)
                    {
                        // Проверка на номер телефона (на повторение)
                        if (context.Clients.Any(r => r.Phone == Phone && r.IdClient != client.IdClient))
                        {
                            errorsList.Add("Введенный номер телефона уже существует. Используйте другой.");
                        }

                        // Проверка на паспорт (на повторение)
                        else if (context.Clients.Any(r => r.Passport == Passport && r.IdClient != client.IdClient))
                        {
                            errorsList.Add("Введенный паспорт уже существует. Используйте другой.");
                        }

                        // Проверка на электронную почту (на повторение)
                        else if (context.Clients.Any(r => r.Email == Email && r.IdClient != client.IdClient))
                        {
                            errorsList.Add("Введенный адрес электронной почты уже существует. Используйте другой.");
                        }
                    }
                    // При добавлении
                    else
                    {
                        // Проверка на номер телефона (на повторение)
                        if (context.Clients.Any(r => r.Phone == Phone))
                        {
                            errorsList.Add("Введенный номер телефона уже существует. Используйте другой.");
                        }

                        // Проверка на паспорт (на повторение)
                        else if (context.Clients.Any(r => r.Passport == Passport))
                        {
                            errorsList.Add("Введенный паспорт уже существует. Используйте другой.");
                        }

                        // Проверка на электронную почту (на повторение)
                        else if (context.Clients.Any(r => r.Email == Email))
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

        #endregion

        #region Услуги
        public static bool LimitatorPrice(Price? price, string name, int cost)
        {
            using (var context = new RealtorsFirmContext())
            {
                // Создаем лист для исключений
                List<string> errorsList = new List<string>();

                // Проверка на пустые поля
                if (string.IsNullOrWhiteSpace(name))
                {
                    errorsList.Add("Заполните поле с наименованием.");
                }
                else if (cost == 0)
                {
                    errorsList.Add("Стоимость не может быть бесплатной. Укажите цену.");
                }
                // Все остальные проверки
                else
                {
                    // При редактировании
                    if (price != null)
                    {
                        if (context.Prices.Any(r => r.Name == name && r.IdPrice != price.IdPrice))
                        {
                            errorsList.Add("Ввденное название услуги уже существует. Используйте другое");
                        }
                    }
                    // При добавлении
                    else
                    {
                        // Проверка на номер телефона (на повторение)
                        if (context.Prices.Any(r => r.Name == name))
                        {
                            errorsList.Add("Ввденное название услуги уже существует. Используйте другое.");
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
        #endregion

        #region Недвижимость
        public static bool LimitatorEstate(Estate? estate, Client? client, TypeEstate? typeEstate, string address,
            int area, int numberRooms, string cost, byte[]? photo)
        {
            using (var context = new RealtorsFirmContext())
            {
                // Создаем лист для исключений
                List<string> errorsList = new List<string>();

                // Проверка на пустые поля
                if (client is null)
                {
                    errorsList.Add("Выберите клиента.");
                }
                else if (typeEstate is null)
                {
                    errorsList.Add("Выберите тип недвижимости.");
                }
                else if (area == 0)
                {
                    errorsList.Add("Площадь недвижимости не может быть нулевой. " +
                        "Укажите корректную площадь недвижимости.");
                }
                else if (numberRooms == 0)
                {
                    errorsList.Add("Количество комнат не может быть нулевым значением. " +
                        "Укажите корректное количество комнат.");
                }
                else if (string.IsNullOrWhiteSpace(cost))
                {
                    errorsList.Add("Стоимость не может быть бесплатной. Укажите цену.");
                }
                else if (string.IsNullOrWhiteSpace(address))
                {
                    errorsList.Add("Укажите адресс недвижимости.");
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
        #endregion

        #region Транзакции
        public static bool LimitatorTransaction(DateTime dateTime, Client client, Employee employee)
        {
            using (var context = new RealtorsFirmContext())
            {
                // Создаем лист для исключений
                List<string> errorsList = new List<string>();

                // Проверка на имя
                if (dateTime.Equals(DateTime.MinValue))
                {
                    errorsList.Add("Укажите корректный формат для даты.");
                }
                // Проверка на возраст (на валидную дату)
                else if (dateTime.Day < DateTime.Now.Day)
                {
                    errorsList.Add("Невозможно записаться на оказание услуг на дату раньше текущей. Выберите день позже текущего.");
                }
                // Проверка на возраст (на валидную дату)
                else if (dateTime > DateTime.Now.AddYears(1))
                {
                    errorsList.Add("Невозможно записаться на оказание услуг на год позже текущего. Выберите день раньше выбранного.");
                }
                // Проверка на сущ транзакцию клиент
                else if (context.Transactions.Any(r => r.IdClient == client.IdClient && r.IdEmployee == employee.IdEmployee && 
                r.DateStart.Year == dateTime.Year &&
                r.DateStart.Month == dateTime.Month &&
                r.DateStart.Day == dateTime.Day &&
                (r.IdStatus == 4 ||
                r.IdStatus == 1)))
                {
                    errorsList.Add($"На выбранный день с риелтором {employee.FullName} и клиентом {client.FullName}" +
                        $" уже существует транзакция Выберите другой день или измените выбор данных.");
                }
                // Проверка на сущ транзакцию клиент
                else if (context.Transactions.Any(r => r.IdEmployee == employee.IdEmployee &&
                r.DateStart.Year == dateTime.Year &&
                r.DateStart.Month == dateTime.Month &&
                r.DateStart.Day == dateTime.Day &&
                (r.IdStatus == 4 ||
                r.IdStatus == 1)))
                {
                    errorsList.Add($"На выбранный день с риелтором {employee.FullName}" +
                        $" уже существует транзакция Выберите другой день или измените выбор данных.");
                }
                // Проверка на сущ транзакцию сотрудник
                else if (context.Transactions.Any(r => r.IdClient == client.IdClient &&
                r.DateStart.Year == dateTime.Year &&
                r.DateStart.Month == dateTime.Month &&
                r.DateStart.Day == dateTime.Day &&
                (r.IdStatus == 4 ||
                r.IdStatus == 1)))
                {
                    errorsList.Add($"На выбранный день с клиентом {client.FullName}" +
                        $" уже существует транзакция Выберите другой день или измените выбор данных.");
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
        #endregion

        #region Вспомогательные методы
        private static short CalculateAge(DateOnly birthDate)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - birthDate.Year;

            if (today.Month < birthDate.Month || (today.Month == birthDate.Month && today.Day < birthDate.Day))
            {
                age--;
            }

            return (short)age;
        }
        #endregion

        #region Ограничения

        // Ограничение по возрасту 
        public static bool LimitatorAge(DateOnly date)
        {
            short age = CalculateAge(date);

            if (age < 18 || age > 80)
            {
                return false;
            }
            return true;
        }

        // Ограничение на валидность почты
        public static bool LimitatorEmail(string email)
        {
            var regex = new Regex(@"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,4}$");
            if (!regex.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        // Ограничение на имя
        public static bool LimitatorName(string name)
        {
            if (name.Contains('-'))
            {
                var regex = new Regex(@"^[а-яА-Я]+\-[а-яА-Я]+$");
                if (!regex.IsMatch(name))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
