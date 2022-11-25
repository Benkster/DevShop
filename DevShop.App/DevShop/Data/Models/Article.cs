using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Article
{
    public int ArticleNr { get; set; }

    public int ProductNr { get; set; }

    public int ProductGroupNr { get; set; }

    public string CompCode { get; set; } = null!;

    public string Article1 { get; set; } = null!;

    public string? ArticleDescription { get; set; }

    public string? ArticleCode { get; set; }

    public string? Ean { get; set; }

    public int SortNr { get; set; }

    public int UnitAmount { get; set; }

    public string PackagingUnit { get; set; } = null!;

    public string BillingUnit { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? Discount { get; set; }

    public bool? OverruleUserDiscount { get; set; }

    public int ArticleHeaderId { get; set; }

    public string? F1 { get; set; }

    public string? F2 { get; set; }

    public string? F3 { get; set; }

    public string? F4 { get; set; }

    public string? F5 { get; set; }

    public string? F6 { get; set; }

    public virtual ArticleHeader ArticleHeader { get; set; } = null!;

    public virtual Unit BillingUnitNavigation { get; set; } = null!;

    public virtual Unit PackagingUnitNavigation { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<UserDiscount> UserDiscounts { get; } = new List<UserDiscount>();
}
