using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class Manufacturer
{
    public int PkId { get; set; }

    public string ManufacturerName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
