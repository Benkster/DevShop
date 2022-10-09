using System.ComponentModel.DataAnnotations;

namespace DevShop.Models
{
	/// <summary>
	/// Companies represent the service providers, the products belong to
	/// </summary>
	public class Company
	{
		[Key]
		[Required]
		[Display(Name = "Company-Code")]
		[StringLength(7, MinimumLength = 7)]
		public string CompCode { get; set; } = string.Empty;

		[Required]
		[Display(Name = "Company")]
		[StringLength(250)]
		public string CompName { get; set; } = string.Empty;

		[Display(Name = "Company - 2nd line")]
		[StringLength(250)]
		public string? CompAddName { get; set; }

		[Display(Name = "Description")]
		[DataType(DataType.MultilineText)]
		[StringLength(500)]
		public string? CompDescr { get; set; }

		[DataType(DataType.PhoneNumber)]
		[StringLength(30)]
		public string? Tel { get; set; }

		[StringLength(30)]
		public string? Fax { get; set; }

		[DataType(DataType.EmailAddress)]
		[Display(Name = "E-Mail")]
		[StringLength(60)]
		public string? Mail { get; set; }

		[DataType(DataType.Url)]
		[StringLength(100)]
		public string? Website { get; set; }



		#region Many-To-Many Connections
		public virtual ICollection<Address> Addresses { get; set; }
		#endregion
	}
}
