using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class PickupPoint
{
    public int PkId { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
