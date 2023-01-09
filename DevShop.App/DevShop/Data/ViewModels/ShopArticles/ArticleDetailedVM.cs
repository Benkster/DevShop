namespace DevShop.Data.ViewModels.ShopArticles
{
    /// <summary>
    /// This class acts as a view-model for articles, that are beeing displayed in the shop.
    /// It provides very detailed information about the article.
    /// </summary>
	public class ArticleDetailedVM
	{
		public int ArticleNr { get; set; }

		public int CategoryID { get; set; }

		public string CategoryName { get; set; } = null!;

		public int ProductNr { get; set; }

		public int ProductGroupNr { get; set; }

		public string CompCode { get; set; } = null!;

		public string CompName { get; set; } = null!;

		public string ArticleName { get; set; } = null!;

		public string? ArticleDescription { get; set; }

		public string? ArticleCode { get; set; }

		public string? Ean { get; set; }

		public int SortNr { get; set; }

		public int UnitAmount { get; set; }

		public string PackagingUnitShort { get; set; } = null!;

		public string PackagingUnit { get; set; } = null!;

		public string BillingUnitShort { get; set; } = null!;

		public string BillingUnit { get; set; } = null!;

		public decimal Price { get; set; }

		public decimal? Discount { get; set; }

		public string? F1Name { get; set; }

		public string? F1 { get; set; }

		public string? F2Name { get; set; }

		public string? F2 { get; set; }

		public string? F3Name { get; set; }

		public string? F3 { get; set; }

		public string? F4Name { get; set; }

		public string? F4 { get; set; }

		public string? F5Name { get; set; }

		public string? F5 { get; set; }

		public string? F6Name { get; set; }

		public string? F6 { get; set; }

		public string PicSource { get; set; } = null!;

		public bool PicExists { get; set; }

		public string Link { get; set; } = null!;
	}
}
