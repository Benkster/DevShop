using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Role
{
    public int RoleNr { get; set; }

    public string Role1 { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
