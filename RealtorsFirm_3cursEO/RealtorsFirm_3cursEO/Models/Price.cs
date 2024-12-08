using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Price
{
    public int IdPrice { get; set; }

    public int Name { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
