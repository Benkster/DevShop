using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string CountryCode { get; set; } = null!;

    public string Country1 { get; set; } = null!;

    public virtual ICollection<State> States { get; } = new List<State>();
}
