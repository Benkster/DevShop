using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// A User serves two purposes:
	/// 1. Administrators can log in in order to gain access to the backend
	/// 2. Customers can log in to receive a special discount for certain articles
	/// </summary>
	public class User
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserID { get; set; }

		[Required]
		[Display(Name = "First name")]
		[StringLength(150)]
		public string FirstName { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Last name")]
		[StringLength(150)]
		public string LastName { get; set; } = string.Empty;

		[Display(Name = "Title ...")]
		[StringLength(50)]
		public string? PreTitle { get; set; }

		[Display(Name = "... Title")]
		[StringLength(50)]
		public string? PostTitle { get; set; }

		[Required]
		[Display(Name = "E-Mail")]
		[StringLength(130)]
		[DataType(DataType.EmailAddress)]
		public string Mail { get; set; } = string.Empty;

		[StringLength(40)]
		[DataType(DataType.PhoneNumber)]
		public string? Tel { get; set; }

		[Required]
		[ForeignKey("RoleNr")]
		public int RoleNr { get; set; }



		#region Foreign-Keys
		public Role Role { get; set; }
		#endregion
	}
}
