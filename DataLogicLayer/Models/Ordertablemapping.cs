using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Ordertablemapping
{
    public long Id { get; set; }

    public long Orderid { get; set; }

    public long Tableid { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
