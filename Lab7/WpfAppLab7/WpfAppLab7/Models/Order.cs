using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class Order
{
    public int PkId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public int FkPickupAddress { get; set; }

    public int FkUserId { get; set; }

    public double PickupCode { get; set; }

    public int FkOrderStatusId { get; set; }

    public virtual OrderStatus FkOrderStatus { get; set; } = null!;

    public virtual PickupPoint FkPickupAddressNavigation { get; set; } = null!;

    public virtual User FkUser { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
