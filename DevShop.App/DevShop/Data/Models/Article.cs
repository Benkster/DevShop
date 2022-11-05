using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace DevShop.Models
{
	/// <summary>
	/// An Article is what a user can add to the wishlist or buy
	/// </summary>
	public class Article
	{
		// Key
		[Required]
		[Display(Name = "Article-Nr")]
		public int ArticleNr { get; set; }

		// Key
		[Required]
		[ForeignKey("ProductNr")]
		public int ProductNr { get; set; }

		// Key
		[Required]
		[Display(Name = "Company-Code")]
		[StringLength(7, MinimumLength = 6)]
		[ForeignKey("CompCode")]
		public string CompCode { get; set; } = string.Empty;

		[Required]
		[ForeignKey("ProductGroupNr")]
		public int ProductGroupNr { get; set; }

		[Required]
		[StringLength(150)]
		[Display(Name = "Article")]
		public string ArticleName { get; set; } = string.Empty;

		[StringLength(800)]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		public string? ArticleDescr { get; set; }

		[StringLength(30)]
		[Display(Name = "Article-Nr")]
		public string? ArticleCode { get; set; }

		[StringLength(13)]
		public string? EAN { get; set; }

		[Required]
		[Display(Name = "Sort-Nr")]
		public int SortNr { get; set; }

		[Required]
		[StringLength(3, MinimumLength = 3)]
		[Display(Name = "Billing-Unit")]
		[ForeignKey("UnitCode")]
		public string BillingUnit { get; set; } = string.Empty;

		[Required]
		[StringLength(3, MinimumLength = 3)]
		[Display(Name = "Order-Unit")]
		[ForeignKey("UnitCode")]
		public string OrderUnit { get; set; } = string.Empty;

		[StringLength(150)]
		public string ?F1 { get; set; }

		[StringLength(150)]
		public string? F2 { get; set; }

		[StringLength(150)]
		public string? F3 { get; set; }

		[StringLength(150)]
		public string? F4 { get; set; }

		[StringLength(150)]
		public string? F5 { get; set; }

		[Required]
		[DataType(DataType.Currency)]
		[Range(0.01, 9999.99)]
		public float Price { get; set; }

		[Range(0.00, 1.00)]
		public float? Discount { get; set; }

		[Required]
		[Display(Name = "Overrule individual discounts for users")]
		public bool OverruleUserDiscount { get; set; } = false;



		#region Foreign-Keys
		public Product Product { get; set; }

		public Unit Unit { get; set; }

		public Company Company { get; set; }
		#endregion
	}
}
