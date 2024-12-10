using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class RoleEmployee
{
    public int IdRole { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
