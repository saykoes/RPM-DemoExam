using System;
using System.Collections.Generic;

namespace WpfAppLab7.Models;

public partial class Category
{
    public int PkId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
