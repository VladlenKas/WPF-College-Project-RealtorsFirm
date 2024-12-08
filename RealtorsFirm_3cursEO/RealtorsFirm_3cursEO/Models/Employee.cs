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

    public string? FullName => $"{Firstname} {Name} {Patronymic}";

    public DateOnly Birthday { get; set; }

    public string Passport { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public ulong? IsDelected { get; set; }

    public virtual RoleEmployee IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
