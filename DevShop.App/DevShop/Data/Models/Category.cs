using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<ProductGroup> ProductGroups { get; } = new List<ProductGroup>();
}
