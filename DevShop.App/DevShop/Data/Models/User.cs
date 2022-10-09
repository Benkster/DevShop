using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevShop.Models
{
	/// <summary>
	/// A User serves two purposes:
	/// 1. Administrators can log in in order to gain access to the backend
	/// 2. Customers can log in to receive a special discount for certain articles
	/// </summary>
	public class User : IdentityUser
	{
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
		[ForeignKey("RoleNr")]
		public int RoleNr { get; set; }



		#region Foreign-Keys
		public Role Role { get; set; }
		#endregion
	}
}
