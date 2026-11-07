using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class Product
{
    public string PkId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string Measurement { get; set; } = null!;

    public double? Price { get; set; }

    public int? FkSupplierId { get; set; }

    public int? FkManufacturerId { get; set; }

    public int? FkCategoryId { get; set; }

    public double? Discount { get; set; }

    public double Quantity { get; set; }

    public string? Description { get; set; }

    public string? Image { get; set; }

    public virtual Category? FkCategory { get; set; }

    public virtual Manufacturer? FkManufacturer { get; set; }

    public virtual Supplier? FkSupplier { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
