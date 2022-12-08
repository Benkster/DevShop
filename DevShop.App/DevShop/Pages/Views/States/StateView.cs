using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Client;

namespace DevShop.Pages.Views.States
{
	/// <summary>
	/// Class for the State-View.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class StateView
    {
        #region Variables
        // Indicates, whether the user has selected a country yet or not
        private bool countryChanged = false;

        // Stores the data of the selected country
        private Country selCountry;

        // A list of all countries
        private List<Country> countries;
        // A list of all states in the selected country
        private List<State> states;



        // The PK of a country (if the user changes the selected country in the edit-view, he will be redirected here and the country he selected should also be selected here)
        [Parameter]
        public string CountryCode { get; set; }
        #endregion



        #region Main Methods
        /// <summary>
        /// Called automatically, when the page loads.
        /// Sets variables needed for the view.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Get all countries from the database and sort them by name
            countries = await uow.CountryRepo.GetAllModelsAsync();
            countries = countries.OrderBy(c => c.CountryName).ToList();


            // The user changed the selected country in the edit-view
            if (!string.IsNullOrEmpty(CountryCode))
            {
                // Get the country, that the user selected in the edit-view
                selCountry = await uow.CountryRepo.GetModelByPkAsync(CountryCode);

                // Get all states of the country, the user selected
                states = await uow.StateRepo.GetAllModelsAsync(CountryCode);
            }
        }
        #endregion



        #region Side Methods
        /// <summary>
        /// Changes the selected country, which also reloads all states, that belong to the newly selected country
        /// </summary>
        private async Task ChangeCountry(ChangeEventArgs args)
        {
            // Prevent the select-element of the countries from reloading, as this would deselect the country
            countryChanged = true;


            // A country has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                selCountry = await uow.CountryRepo.GetModelByPkAsync(args.Value.ToString());
                states = await uow.StateRepo.GetAllModelsAsync(args.Value.ToString());
            }
            // The option "Please select a country" has been selected
            else
            {
                states.Clear();
                selCountry = null;
            }


            // Update the view
            StateHasChanged();
        }



        /// <summary>
        /// If a state is beeing selected, redirect to the edit-view of this state
        /// </summary>
        private void ChangeState(ChangeEventArgs args)
        {
            // A state has been selected (0 = "Please select a state")
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && selCountry != null)
            {
                nav.NavigateTo("/state/edit/" + selCountry.CountryCode + "/" + args.Value.ToString());
            }
        }
        #endregion
    }
}
