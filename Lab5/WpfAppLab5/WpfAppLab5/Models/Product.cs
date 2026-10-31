using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class Product
{
    public string PkId { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string UnitName { get; set; } = null!;

    public double? Price { get; set; }

    public int FkSupplierId { get; set; }

    public int FkManufacturerId { get; set; }

    public int FkCategoryId { get; set; }

    public double? Discount { get; set; }

    public double Quantity { get; set; }

    public string? Description { get; set; }

    public string? ImagePath { get; set; }

    public virtual Category FkCategory { get; set; } = null!;

    public virtual Manufacturer FkManufacturer { get; set; } = null!;

    public virtual Supplier FkSupplier { get; set; } = null!;
}
