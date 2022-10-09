using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// States are used for addresses of companies/users
	/// E.g. Tirol
	/// </summary>
	public class State
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int StateID { get; set; }

		[Required]
		[Display(Name = "State")]
		[StringLength(150)]
		public string StateName { get; set; } = string.Empty;

		[Required]
		[ForeignKey("CountryID")]
		public int CountryID { get; set; }

		[Required]
		[StringLength(3)]
		[ForeignKey("CountryCode")]
		public string CountryCode { get; set; } = string.Empty;



		#region Foreign-Keys
		public Country Country { get; set; }
		#endregion
	}
}
