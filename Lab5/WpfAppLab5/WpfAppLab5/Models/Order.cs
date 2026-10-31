using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class Order
{
    public int PkId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? PickupDate { get; set; }

    public int FkPickupPointId { get; set; }

    public int FkUserId { get; set; }

    public int? Code { get; set; }

    public int FkOrderStatusId { get; set; }

    public virtual OrderStatus FkOrderStatus { get; set; } = null!;

    public virtual PickupPoint FkPickupPoint { get; set; } = null!;

    public virtual User FkUser { get; set; } = null!;
}
