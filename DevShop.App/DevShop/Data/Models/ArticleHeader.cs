using System;
using System.Collections.Generic;

namespace DevShop.Data.Models;

public partial class ArticleHeader
{
    public int ArticleHeaderId { get; set; }

    public int ProductNr { get; set; }

    public int ProductGroupNr { get; set; }

    public string CompCode { get; set; } = null!;

    public string? F1name { get; set; }

    public string? F2name { get; set; }

    public string? F3name { get; set; }

    public string? F4name { get; set; }

    public string? F5name { get; set; }

    public string? F6name { get; set; }

    public virtual ICollection<Article> Articles { get; } = new List<Article>();

    public virtual Product Product { get; set; } = null!;
}
