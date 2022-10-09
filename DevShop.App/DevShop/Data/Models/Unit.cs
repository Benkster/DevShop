using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// The Units secify in which way an article can be bought.
	/// E.g. can be bought in pieces or in litres
	/// </summary>
	public class Unit
	{
		[Key]
		[Required]
		[Display(Name = "Unit-Code")]
		[StringLength(3, MinimumLength = 3)]
		public string UnitCode { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Unit")]
		[StringLength(50)]
		public string UnitName { get; set; } = string.Empty;
	}
}
