using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// Cities are used for addresses of companies/users
	/// E.g. Ebbs
	/// </summary>
	public class City
	{
		// Key
		[Required]
		[DataType(DataType.PostalCode)]
		[StringLength(5)]
		public string ZIP { get; set; } = string.Empty;

		// Key
		[Required]
		[ForeignKey("StateID")]
		public int StateID { get; set; }

		[Required]
		[StringLength(150)]
		[Display(Name = "City")]
		public string CityName { get; set; } = string.Empty;



		#region Foreign-Keys
		public State State { get; set; }
		#endregion
	}
}
