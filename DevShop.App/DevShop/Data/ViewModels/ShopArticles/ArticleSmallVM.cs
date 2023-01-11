namespace DevShop.Data.ViewModels.ShopArticles
{
    /// <summary>
    /// This class acts as a view-model for articles, that are beeing displayed at the entry page of the shop.
    /// It only provides few information of an article.
    /// </summary>
    public class ArticleSmallVM
    {
        public int ArticleNr { get; set; }

        public int ProductNr { get; set; }

        public string CompCode { get; set; } = null!;

        public string ArticleName { get; set; } = null!;

        public string? ArticleCode { get; set; }

        public string? Ean { get; set; }

        public decimal Price { get; set; }

        public string PicSource { get; set; } = null!;

        public string Link { get; set; } = null!;
    }
}
