using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class UserDiscount
{
    public int UserDiscountId { get; set; }

    public int UserId { get; set; }

    public int ArticleNr { get; set; }

    public int ProductNr { get; set; }

    public int ProductGroupNr { get; set; }

    public string CompCode { get; set; } = null!;

    public decimal Discount { get; set; }

    public virtual Article Article { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
