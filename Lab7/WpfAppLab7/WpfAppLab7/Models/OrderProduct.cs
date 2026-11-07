using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class OrderProduct
{
    public int FkOrderId { get; set; }

    public string FkProductId { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual Order FkOrder { get; set; } = null!;

    public virtual Product FkProduct { get; set; } = null!;
}
