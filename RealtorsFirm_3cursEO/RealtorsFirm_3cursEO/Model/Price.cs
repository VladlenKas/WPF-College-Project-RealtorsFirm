using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Price
{
    public int IdPrice { get; set; }

    public string Name { get; set; } = null!;

    public int Cost { get; set; }

    public string NameCost => $"{Name}. Цена: {Cost} руб.";

    public sbyte? IsDeleted { get; set; }

    public sbyte? IsArchive { get; set; }

    public virtual ICollection<TransactionPriceRelation> TransactionPriceRelations { get; set; } = new List<TransactionPriceRelation>();
}
