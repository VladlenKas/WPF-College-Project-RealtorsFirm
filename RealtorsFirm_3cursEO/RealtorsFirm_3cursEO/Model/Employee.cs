using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public int IdRole { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public string Passport { get; set; } = null!;

    public string FIPassport => $"{Firstname} {Name}, {Passport.Insert(4, " ")}";

    public string FIPhone // Свойство, чтобы передавать клиента для лучшего поиска
    {
        get
        {
            string phone = Phone.Insert(1, "-").Insert(5, "-").Insert(9, "-").Insert(12, "-");
            return $"{Firstname} {Name}, {phone}";
        }
    }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public sbyte? IsDeleted { get; set; }

    public sbyte? IsArchive { get; set; }

    public virtual RoleEmployee IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    // Свойство для контроля переноса текста
    public bool IsTextWrapped
    {
        get
        {
            // Логика определения необходимости переноса текста
            return FullName.Length > 35 ||
                Email.Length > 20 ||
                Password.Length > 20; // Пример: перенос, если длина больше 40 символов
        }
    }
}
