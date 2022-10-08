using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// As the name suggests, they group products that are similar.
	/// E.g. glasses and sunglasses are two different products but can and should be part of the same group
	/// </summary>
	public class ProductGroup
	{
		[Key]
		[Required]
		[Display(Name = "Product-Group-Nr")]
		public int ProductGroupNr { get; set; }

		[Key]
		[Required]
		[StringLength(7, MinimumLength = 7)]
		[ForeignKey("CompCode")]
		public string CompCode { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Group-Name")]
		[StringLength(150)]
		public string GroupName { get; set; } = string.Empty;

		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		[StringLength(500)]
		public string? GroupDescr { get; set; }

		[Display(Name = "Sort-Nr")]
		[Required]
		public int SortNr { get; set; }

		public int? ParentNr { get; set; }

		[Required]
		[ForeignKey("CategoryID")]
		public int CategoryID { get; set; }



		#region Foreign-Keys
		public Company Company { get; set; }

		public Category Category { get; set; }
		#endregion
	}
}
