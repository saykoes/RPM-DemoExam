using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class OrderStatus
{
    public int PkId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
