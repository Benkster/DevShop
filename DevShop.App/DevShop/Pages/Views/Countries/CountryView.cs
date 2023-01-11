using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Countries
{
	/// <summary>
	/// Class for the Country-View.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class CountryView
	{
		#region Variables
		// List of all existing countries
		private List<Country> countries;
		#endregion



		#region Main Methods
		/// <summary>
		/// Called automatically, when the page loads.
		/// Sets variables needed for the view.
		/// </summary>
		protected override async Task OnInitializedAsync()
		{
			countries = await uow.CountryRepo.GetAllModelsAsync();
		}
		#endregion



		#region Side Methods
		/// <summary>
		/// If the user selects a country, redirect to the edit-view
		/// </summary>
		private void ChangeCountry(ChangeEventArgs args)
		{
			// Only proceed, if a country has been selected (0 = "Please select a country")
			if (args.Value != null && args.Value.ToString() != "0")
			{
				nav.NavigateTo("/country/edit/" + args.Value.ToString());
			}
		}
		#endregion
	}
}
