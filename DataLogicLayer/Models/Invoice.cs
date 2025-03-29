using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Invoice
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public string? InvoiceNo { get; set; }

    public virtual ICollection<Invoicetaxmapping> Invoicetaxmappings { get; set; } = new List<Invoicetaxmapping>();

    public virtual Order Order { get; set; } = null!;
}
