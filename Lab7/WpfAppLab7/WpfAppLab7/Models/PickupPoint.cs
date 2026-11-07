using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class PickupPoint
{
    public int PkId { get; set; }

    public string PickupAddress { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
