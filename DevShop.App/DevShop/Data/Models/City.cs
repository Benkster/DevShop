using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class City
{
    public int StateId { get; set; }

    public string Zip { get; set; } = null!;

    public string City1 { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; } = new List<Address>();

    public virtual State State { get; set; } = null!;
}
