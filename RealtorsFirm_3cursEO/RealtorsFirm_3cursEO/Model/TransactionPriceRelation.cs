using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class TransactionPriceRelation
{
    public int IdTransactionPriceRelations { get; set; }

    public int IdTransaction { get; set; }

    public int IdPrice { get; set; }

    public virtual Price IdPriceNavigation { get; set; } = null!;

    public virtual Transaction IdTransactionNavigation { get; set; } = null!;
}
