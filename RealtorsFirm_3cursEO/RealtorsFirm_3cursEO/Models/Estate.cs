using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Estate
{
    public int IdEstate { get; set; }

    public int? IdClient { get; set; }

    public int IdType { get; set; }

    public string Address { get; set; } = null!;

    public int Area { get; set; }

    public int NumberRooms { get; set; }

    public string Cost { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public ulong? IsDelected { get; set; }

    public virtual Client? IdClientNavigation { get; set; }

    public virtual TypeEstate IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
