using DevShop.Data;
using DevShop.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Articles.Edit
{
	/// <summary>
	/// Handles data for the corresponding view
	/// </summary>
	public partial class CompanyEditView
	{
		#region Variables/Properties
		// Determine, whether a new company is beeing created or an existing one is beeing edited
		private bool _edit = false;
		// True, If the form gets submitted but the users input is invalid
		private bool _invalid = false;

		private Company company = new Company();



		// If a company is beeing edited, this parameter contains the PK to get the data from the DB
		[Parameter]
		public string? CompCode { get; set; }
		#endregion



		#region Methods
		/// <summary>
		/// Called automatically when page is loaded.
		/// Sets variables that are needed for loading the view
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			// If a parameter has been passed, an existing company should be edited
			if (!string.IsNullOrEmpty(CompCode))
			{
				_edit = true;

				// Get the data of the company from the DB
				company = await uow.CompanyRepo.GetCompanyByIdAsync(CompCode);
			}
			// A new company should be created
			else
			{
				company = new Company();
			}
		}



		/// <summary>
		/// Creates a new entry in the DB or updates an existing one
		/// </summary>
		private async void Save()
		{
			if (_edit)
			{
				await uow.CompanyRepo.UpdateCompanyAsync(company);
			}
			else
			{
				await uow.CompanyRepo.CreateNewCompanyAsync(company);
			}


			nav.NavigateTo("/company");
		}



		/// <summary>
		/// Deletes an entry from the DB
		/// </summary>
		private async Task Delete()
		{
			await uow.CompanyRepo.DeleteCompanyAsync(CompCode);
		}



		/// <summary>
		/// The form is beeing submitted but the given data is invalid.
		/// Triggers an error that is beeing displayed on the view.
		/// </summary>
		private void InvalidSubmit()
		{
			_invalid = true;
		}
		#endregion
	}
}
