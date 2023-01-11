using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? PreTitle { get; set; }

    public string? PostTitle { get; set; }

    public string Password { get; set; } = null!;

    public string Mail { get; set; } = null!;

    public string? Tel { get; set; }

    public int RoleNr { get; set; }

    public string? AuthCookie { get; set; }

    public DateTime? LastLogin { get; set; }

    public string CompCode { get; set; } = null!;

    public virtual Company CompCodeNavigation { get; set; } = null!;

    public virtual Role RoleNrNavigation { get; set; } = null!;

    public virtual ICollection<UserDiscount> UserDiscounts { get; } = new List<UserDiscount>();
}
