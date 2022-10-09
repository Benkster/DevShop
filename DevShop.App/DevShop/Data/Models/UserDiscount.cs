using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// Every user can have individual discounts on certain articles
	/// </summary>
	public class UserDiscount
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserDiscountID { get; set; }

		[Required]
		[ForeignKey("ProductNr")]
		public int ProductNr { get; set; }

		[Required]
		[ForeignKey("ArticleNr")]
		public int ArticleNr { get; set; }

		[Required]
		[Range(0.00, 1.00)]
		public float? Discount { get; set; }



		#region Foreign-Keys
		public User User { get; set; }

		public Article Article { get; set; }
		#endregion
	}
}
