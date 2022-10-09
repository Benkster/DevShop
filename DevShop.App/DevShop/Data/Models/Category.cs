using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// Product-Groups can be categorized in order to uphold
	/// a good overview and to make it easier for the customers
	/// to find the article of their needs.
	/// </summary>
	public class Category
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CategoryID { get; set; }

		[Required]
		[StringLength(150)]
		[Display(Name = "Category")]
		public string CategoryName { get; set; } = string.Empty;

		[StringLength(350)]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		public string? CategoryDescr { get; set; }

		public int? ParentID { get; set; }
	}
}
