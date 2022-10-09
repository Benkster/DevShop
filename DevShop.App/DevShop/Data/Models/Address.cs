using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace DevShop.Models
{
	/// <summary>
	/// Stores all availlable addresses.
	/// If an address already exists, it should not be created a second time.
	/// </summary>
	public class Address
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int AddressID { get; set; }

		[Required]
		[ForeignKey("StateID")]
		public int StateID { get; set; }

		[Required]
		[StringLength(5)]
		[DataType(DataType.PostalCode)]
		[ForeignKey("ZIP")]
		public string ZIP { get; set; } = string.Empty;

		[Required]
		[StringLength(150)]
		public string Street { get; set; } = string.Empty;

		[Required]
		[StringLength(20)]
		[Display(Name = "House-Nr")]
		public string HouseNr { get; set; } = string.Empty;

		[StringLength(300)]
		[DataType(DataType.MultilineText)]
		[Display(Name = "Address information")]
		public string? AddressInfo { get; set; }



		#region Foreign-Keys
		public City City { get; set; }
		#endregion


		#region Many-To-Many Connections
		public virtual ICollection<Company> Companies { get; set; }
		#endregion
	}
}
