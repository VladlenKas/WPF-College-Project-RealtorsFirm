using System;
using System.Collections.Generic;

namespace RealtorsFirm_3cursEO.Model;

public partial class Estate
{
    public int IdEstate { get; set; }

    public int IdClient { get; set; }

    public int IdType { get; set; }

    public string Address { get; set; } = null!;

    public int Area { get; set; }

    public int NumberRooms { get; set; }

    public string Cost { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public sbyte? IsDeleted { get; set; }

    public sbyte? IsArchive { get; set; }

    public string IdWithAddress => $"ID: {IdEstate}. {Address}";

    public virtual Client IdClientNavigation { get; set; } = null!;

    public virtual TypeEstate IdTypeNavigation { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    // Свойство для контроля переноса текста
    public bool IsTextWrapped
    {
        get
        {
            // Логика определения необходимости переноса текста
            return Address.Length > 40;
        }
    }
}
