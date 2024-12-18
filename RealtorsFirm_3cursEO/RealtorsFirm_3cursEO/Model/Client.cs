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

    public string Passport { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public sbyte? IsDeleted { get; set; }

    public sbyte? IsArchive { get; set; }

    public int Bonuses { get; set; }

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();
}
