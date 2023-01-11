using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class State
{
    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string CountryCode { get; set; } = null!;

    public string StateName { get; set; } = null!;

    public virtual ICollection<City> Cities { get; } = new List<City>();

    public virtual Country Country { get; set; } = null!;
}
