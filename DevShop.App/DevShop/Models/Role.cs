using System.ComponentModel.DataAnnotations;

namespace DevShop.Models
{
	/// <summary>
	/// Model that holds the roles, which can be assigned to a user.
	/// Roles determin the rights a user has e.g. login for discounts
	/// or login for the backend
	/// </summary>
	public class Role
	{
		[Key]
		[Required]
		[Display(Name = "Role-Nr")]
		public int RoleNr { get; set; }

		[Required]
		[Display(Name = "Role")]
		[StringLength(100)]
		public string RoleName { get; set; } = string.Empty;

		[DataType(DataType.MultilineText)]
		[Display(Name = "Description")]
		[StringLength(200)]
		public string? RoleDescr { get; set; }
	}
}
