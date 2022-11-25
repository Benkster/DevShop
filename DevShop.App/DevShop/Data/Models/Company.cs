using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Company
{
    public string CompCode { get; set; } = null!;

    public string CompName { get; set; } = null!;

    public string? CompAddName { get; set; }

    public string? Description { get; set; }

    public string? Tel { get; set; }

    public string? Mail { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<ProductGroup> ProductGroups { get; } = new List<ProductGroup>();

    public virtual ICollection<User> Users { get; } = new List<User>();

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();
}
