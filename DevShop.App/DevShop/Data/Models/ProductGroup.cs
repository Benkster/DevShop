using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class ProductGroup
{
    public int ProductGroupNr { get; set; }

    public string CompCode { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string? GroupDescription { get; set; }

    public int SortNr { get; set; }

    public int? ParentId { get; set; }

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Company CompCodeNavigation { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
