using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int StateId { get; set; }

    public string Zip { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string HouseNr { get; set; } = null!;

    public string? Info { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Company> CompCodes { get; } = new List<Company>();
}
