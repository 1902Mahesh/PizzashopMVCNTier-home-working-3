using System;
using System.Collections.Generic;

namespace DataLogicLayer.Models;

public partial class Orderitemsmodifier
{
    public long Id { get; set; }

    public long OrderItemId { get; set; }

    public long ModifierItemId { get; set; }

    public decimal? Rate { get; set; }

    public int Quantity { get; set; }

    public virtual Modifieritem ModifierItem { get; set; } = null!;

    public virtual Orderdetail OrderItem { get; set; } = null!;
}
