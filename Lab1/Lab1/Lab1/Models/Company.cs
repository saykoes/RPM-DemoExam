using System;
using System.Collections.Generic;

namespace Lab1.Models;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> ProductManufacturers { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductProviders { get; set; } = new List<Product>();
}
