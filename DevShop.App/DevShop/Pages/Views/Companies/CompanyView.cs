using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Companies
{
    /// <summary>
    /// Class for the Company-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class CompanyView
	{
		#region Variables
		private List<Company> companies;
		#endregion



		#region Main Methods
		/// <summary>
		/// Called automatically, when the page loads.
		/// Sets variables needed for the view.
		/// </summary>
		protected override async Task OnInitializedAsync()
		{
			companies = await uow.CompanyRepo.GetAllModelsAsync();
			companies = companies.OrderBy(c => c.CompName).OrderBy(c => c.CompCode.Substring(0, 2)).ToList();
		}
		#endregion



		#region Side Methods
		/// <summary>
		/// If the user selects a company, redirect to the edit-view
		/// </summary>
		private void ChangeCompany(ChangeEventArgs args)
		{
			// Only redirect, if an existing company has been selected (0 = "Please select a company")
			if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
			{
				nav.NavigateTo("/company/edit/" + args.Value.ToString());
			}
		}
		#endregion
	}
}
