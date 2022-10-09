using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// A product is a general term for different articles
	/// E.g. sunglasses are a product and the different types of sunglasses 
	/// (difference in size or color) are the articles associated with the product
	/// </summary>
	public class Product
	{
		// Key
		[Required]
		[Display(Name = "Product-Nr")]
		public int ProductNr { get; set; }

		// Key
		[Required]
		[ForeignKey("ProductGroupNr")]
		public int ProductGroupNr { get; set; }

		[Required]
		[StringLength(7, MinimumLength = 7)]
		[ForeignKey("CompCode")]
		public string CompCode { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Product")]
		[StringLength(150)]
		public string ProductName { get; set; } = string.Empty;

		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		[StringLength(800)]
		public string? ProductDescr { get; set; }

		[Display(Name = "Sort-Nr")]
		[Required]
		public int SortNr { get; set; }



		#region Foreign-Keys
		public ProductGroup ProductGroup { get; set; }
		#endregion
	}
}
