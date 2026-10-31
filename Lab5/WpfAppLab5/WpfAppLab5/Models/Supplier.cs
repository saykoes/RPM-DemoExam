using System;
using System.Collections.Generic;

namespace WpfAppLab5.Models;

public partial class Supplier
{
    public int PkId { get; set; }

    public string SupplierName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
