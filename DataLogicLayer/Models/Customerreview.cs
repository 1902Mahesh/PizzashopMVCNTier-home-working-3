using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Customerreview
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public long Customerid { get; set; }

    public decimal? Foodrating { get; set; }

    public decimal? Servicerating { get; set; }

    public decimal? Ambiencerating { get; set; }

    public decimal? AverageRating { get; set; }

    public string? Review { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public bool Isdeleted { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
