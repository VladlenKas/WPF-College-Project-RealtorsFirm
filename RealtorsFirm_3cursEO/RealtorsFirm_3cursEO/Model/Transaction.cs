﻿using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Transaction
{
    public int IdTransaction { get; set; }

    public int IdEmployee { get; set; }

    public int IdClient { get; set; }

    public int IdEstate { get; set; }

    public int IdStatus { get; set; }

    public DateTime Date { get; set; }

    public int Cost { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; } = null!;

    public virtual Estate IdEstateNavigation { get; set; } = null!;

    public virtual StatusTransaction IdStatusNavigation { get; set; } = null!;

    public virtual ICollection<TransactionPriceRelation> TransactionPriceRelations { get; set; } = new List<TransactionPriceRelation>();
}