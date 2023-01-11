using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Companies
{
    /// <summary>
    /// Class for the Company-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class CompanyEditView
	{
		#region Variables/Properties
		// Determines, whether a new company is beeing created, or an existing one is beeing edited
		private bool isEdit;
		// Indicates, whether the user has selected a different company yet or not
		private bool companyChanged = false;

		// An error message that is beeing shown, if the submition of the form was invalid
		private MarkupString errorMessage;

		// The next available CompCode for the company, that is beeing created
		private string nextCompCode;
		// The code of the country, that has been selected
		private string selCountryCode;

		// The company, that is beeing created/edited
		private Company company;

		// A list of all companies
		private List<DropDownVM> dropDownCompanies;
		// A list of all countries
		private List<DropDownVM> dropDownCountries;



		// PK of the company, that is beeing edited
		[Parameter]
		public string CompCode { get; set; }
		#endregion



		#region Main Methods
		/// <summary>
		/// Called, when the parameters of the URL change.
		/// This is needed instead of the OnInitializedAsync, because when the user selects a different company (which changes the parameters of the URL), the displayed data in the form has to update.
		/// </summary>
		protected override async Task OnParametersSetAsync()
		{
			// Get all companies from the DB and sort them by name
			List<Company> companies = await uow.CompanyRepo.GetAllModelsAsync();
			dropDownCompanies = companies.OrderBy(c => c.CompName).OrderBy(c => c.CompCode.Substring(0, 2)).Select(c => new DropDownVM()
            {
				Value = c.CompCode,
				DisplayName = c.CompCode.Substring(0, 2) + " - " + c.CompName
			}).ToList();

			// Get all existing countries and sort them by name
			List<Country> countries = await uow.CountryRepo.GetAllModelsAsync();
			dropDownCountries = countries.OrderBy(c => c.CountryName).Select(c => new DropDownVM()
			{
				Value = c.CountryCode.Substring(0, 2),
				DisplayName = c.CountryName
			}).ToList();


			// Reset the error message
			errorMessage = new MarkupString();


			// An existing company is beeing edited
			if (!string.IsNullOrEmpty(CompCode))
			{
				isEdit = true;
				nextCompCode = string.Empty;
				selCountryCode = CompCode.Substring(0, 2);

				company = await uow.CompanyRepo.GetModelByPkAsync(CompCode);
			}
			// A new company is beeing created
			else
			{
				isEdit = false;
				selCountryCode = dropDownCountries.FirstOrDefault().Value;
				nextCompCode = await uow.CompanyRepo.GetNextPkAsync(selCountryCode);

				company = new Company();
				company.CompCode = nextCompCode;
			}


			// Update the data on the view
			StateHasChanged();
		}



		/// <summary>
		/// Create a new company or updates an existing one
		/// </summary>
		private async Task Save()
		{
			// Update an existing company
			if (isEdit)
			{
				await uow.CompanyRepo.UpdateModelAsync(company);
			}
			// Create a new company
			else
			{
				await uow.CompanyRepo.CreateModelAsync(company);
			}


			nav.NavigateTo("/company");
		}



		/// <summary>
		/// Show an error message, if the form-submit was invalid
		/// </summary>
		private void Error()
		{
			errorMessage = (MarkupString)"Something went wrong. Please try again";
		}



		/// <summary>
		/// Delete a company from the database
		/// </summary>
		private async Task Delete()
		{
			// Only delete it, if a company is beeing edited and not created
			if (isEdit)
			{
				List<User> compUsers = await uow.UserRepo.GetAllModelsAsync(CompCode);


				// If there are no users associated with the company, delete it
				if (compUsers.Count == 0)
				{
					await uow.CompanyRepo.DeleteModelAsync(CompCode);
					nav.NavigateTo("/company");
				}
				// If there are any users associated with the company, the user is not allowed to delete it
				else
				{
					errorMessage = (MarkupString)"There are one or more users associated with this company.<br>Please click <a href=\"./user\">here</a> to remove all users before deleting the company.";
                }
			}
		}
		#endregion



		#region Side Methods
		/// <summary>
		/// If the user changes the selected company, redirect to the edit-view
		/// </summary>
		private async Task ChangeCompany(ChangeEventArgs args)
		{
			// Prevent the select-element of the companies from reloading, as this would deselect the company
			companyChanged = true;


			// Only proceed, if an existing company has been selected (0 = "Please select a company")
			if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
			{
				nav.NavigateTo("/company/edit/" + args.Value.ToString());
			}
		}



		/// <summary>
		/// Set the next available CompCode of the company depending on the selected country
		/// </summary>
		private async Task ChangeCountry(ChangeEventArgs args)
        {
			if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
				// Code of the country, that has been selected
				selCountryCode = args.Value.ToString();
				// Get the next available CompCode
				nextCompCode = await uow.CompanyRepo.GetNextPkAsync(selCountryCode);

				company.CompCode = nextCompCode;


				StateHasChanged();
			}
        }
		#endregion
	}
}
