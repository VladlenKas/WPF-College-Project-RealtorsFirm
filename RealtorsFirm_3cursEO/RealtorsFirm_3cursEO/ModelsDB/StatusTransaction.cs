using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.ModelsDB;

public partial class StatusTransaction
{
    public int IdStatus { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
