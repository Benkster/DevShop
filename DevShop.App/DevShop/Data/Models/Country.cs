using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// Countries are used for addresses of companies/users
	/// E.g. Austria
	/// </summary>
	public class Country
	{
		// Key
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int CountryID { get; set; }

		// Key
		[Required]
		[Display(Name = "Country-Code")]
		[StringLength(3)]
		public string CountryCode { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Country")]
		[StringLength(150)]
		public string CountryName { get; set; } = string.Empty;
	}
}
