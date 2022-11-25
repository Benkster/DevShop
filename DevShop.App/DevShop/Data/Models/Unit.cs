using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Unit
{
    public string UnitCode { get; set; } = null!;

    public string Unit1 { get; set; } = null!;

    public virtual ICollection<Article> ArticleBillingUnitNavigations { get; } = new List<Article>();

    public virtual ICollection<Article> ArticlePackagingUnitNavigations { get; } = new List<Article>();
}
