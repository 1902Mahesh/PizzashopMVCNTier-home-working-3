using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Invoicetaxmapping
{
    public long Id { get; set; }

    public long Invoiceid { get; set; }

    public string? Taxname { get; set; }

    public decimal? Taxvalue { get; set; }

    public bool? TaxType { get; set; }

    public virtual Invoice Invoice { get; set; } = null!;
}
