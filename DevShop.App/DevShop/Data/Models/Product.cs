using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class Product
{
    public int ProductNr { get; set; }

    public int ProductGroupNr { get; set; }

    public string CompCode { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public string? ProductDescription { get; set; }

    public int SortNr { get; set; }

    public virtual ICollection<ArticleHeader> ArticleHeaders { get; } = new List<ArticleHeader>();

    public virtual ICollection<Article> Articles { get; } = new List<Article>();

    public virtual ProductGroup ProductGroup { get; set; } = null!;
}
