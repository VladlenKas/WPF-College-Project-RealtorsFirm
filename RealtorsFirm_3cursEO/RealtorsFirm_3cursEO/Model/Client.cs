using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Client
{
    public int IdClient { get; set; }

    public string Name { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public string? Passport { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? Password { get; set; } 

    public sbyte? IsDeleted { get; set; }

    public sbyte? IsArchive { get; set; }

    public int? Bonuses { get; set; }

    public sbyte? IsRegistered { get; set; }

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();

    // Свойство для контроля переноса текста
    public bool IsTextWrapped
    {
        get
        {
            if (IsRegistered == 1)
            {
                // Логика определения необходимости переноса текста
                return FullName.Length > 40 ||
                    Email.Length > 20 ||
                    Password.Length > 20; // Пример: перенос, если длина больше 40 символов
            }
            else
            {
                return FullName.Length > 40;
            }
        }
    }
}
