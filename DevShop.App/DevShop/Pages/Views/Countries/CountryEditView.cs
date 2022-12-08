using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Security;

namespace DevShop.Pages.Views.Countries
{
	/// <summary>
	/// Class for the Country-Edit-View.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class CountryEditView
	{
		#region Variables/Properties
		// Determines, whether a new country is beeing created or an existing one is beeing updated
		private bool isEdit;

		// This error-message is shown, if the submittion of the form failed/was invalid
		private string errorMessage;

		// The country, that is beeing created/edited
		private Country country;

		// A list of all existing countries
		private List<DropDownVM> dropDownList;



		// PK of the country, that is beeing edited
		[Parameter]
		public string CountryCode { get; set; }
		#endregion



		#region Main Methods
		/// <summary>
		/// Called, when the parameters of the URL change.
		/// This is needed instead of the OnInitializedAsync, because when the user selects an existing country (which changes the parameters of the URL), the displayed data in the form has to update.
		/// </summary>
		protected override async Task OnParametersSetAsync()
		{
			// An existing country is beeing edited
			if (!string.IsNullOrEmpty(CountryCode))
			{
				isEdit = true;
				country = await uow.CountryRepo.GetModelByPkAsync(CountryCode);
			}
			// A new country is beeing created
			else
			{
				isEdit = false;
				country = new Country();
			}


			List<Country> countries = await uow.CountryRepo.GetAllModelsAsync();
			dropDownList = countries.Select(c => new DropDownVM()
			{
				Value = c.CountryCode,
				DisplayName = c.CountryName
			}).ToList();


			// Update the data on the view
			StateHasChanged();
		}



		/// <summary>
		/// Create a new country in the database or update an existing one
		/// </summary>
		private async Task Save()
		{
			// Update an existing country
			if (isEdit)
			{
				await uow.CountryRepo.UpdateModelAsync(country);
			}
			// Create a new country
			else
			{
				await uow.CountryRepo.CreateModelAsync(country);
			}


			// Redirect to overview
			nav.NavigateTo("/country");
		}



		/// <summary>
		/// The form-submition was invalid
		/// </summary>
		private async Task Error()
		{
			errorMessage = "Something went wrong. Please try again";
		}



		/// <summary>
		/// Delete a country from the database
		/// </summary>
		private async Task Delete()
		{
			// Only delete it, if a country is beeing edited and not created
			if (isEdit)
			{
				await uow.CountryRepo.DeleteModelAsync(CountryCode);
				nav.NavigateTo("/country");
			}
		}
		#endregion



		#region Side Methods
		/// <summary>
		/// If the user selects a country, redirect to edit the selected country
		/// </summary>
		private void ChangeCountry(ChangeEventArgs args)
		{
			// Only proceed, if a country has been selected (0 = "Please select a country")
			if (args.Value != null && args.Value.ToString() != "0")
			{
				nav.NavigateTo("/country/edit/" + args.Value.ToString());
			}
			// If the option "Please select a country" was selected, redirect to overview
			else
			{
				nav.NavigateTo("/country");
			}
		}
		#endregion
	}
}
