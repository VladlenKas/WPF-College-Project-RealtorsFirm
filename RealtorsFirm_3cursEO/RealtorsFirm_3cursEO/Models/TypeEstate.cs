using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class TypeEstate
{
    public int IdType { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Estate> Estates { get; set; } = new List<Estate>();
}
