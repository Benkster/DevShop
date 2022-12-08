using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Cities
{
	/// <summary>
	/// Class for the City-View.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class CityView
    {
        #region Variables
        // Indicates, whether the user has selected a country yet or not
        private bool countryChanged = false;
        // Indicates, whether the user has selected a state yet or not
        private bool stateChanged = false;

        // Stores the data of the selected country
        private Country selCountry;
        // Stores the data of the selected state
        private State selState;

        // A list of all countries
        private List<Country> countries;
        // A list of all states in the selected country
        private List<State> states;
        // A lsit of all cities, that belong to the selected state
        private List<City> cities;



        // The PK of a country (if the user changes the selected country in the edit-view, he will be redirected here and the country he selected should also be selected here)
        [Parameter]
        public string CountryCode { get; set; }

		// The PK of the state, (if the user changes the selected state in the edit-view, he will be redirected here and the country and state he selected should also be selected here)
		[Parameter]
        public string StateID { get; set; }
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

                // Get all states of the country, the user selected, and sort them by name
                states = await uow.StateRepo.GetAllModelsAsync(CountryCode);
                states = states.OrderBy(s => s.StateName).ToList();
            }


            // The user changed the selected state in the edit-view
            if (!string.IsNullOrEmpty(StateID))
			{
                // Get the state, that the user selected in the edit-view
                selState = await uow.StateRepo.GetModelByPkAsync(Convert.ToInt32(StateID));

                // Get all cities of the state, the user selected, and sort them by name
                cities = await uow.CityRepo.GetAllModelsAsync(Convert.ToInt32(StateID));
                cities = cities.OrderBy(c => c.CityName).ToList();
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
            // Allow the select-element of the states to reload
            stateChanged = false;


            // A country has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                selCountry = await uow.CountryRepo.GetModelByPkAsync(args.Value.ToString());

                states = await uow.StateRepo.GetAllModelsAsync(args.Value.ToString());
                states = states.OrderBy(s => s.StateName).ToList();
            }
            // The option "Please select a country" has been selected
            else
            {
                states.Clear();
                selCountry = null;
            }


            if (cities != null)
            {
                cities.Clear();
            }

            selState = null;


            // Update the view
            StateHasChanged();
        }



        /// <summary>
        /// Changes the selected state, which also reloads all cities, that belong to the newly selected state
        /// </summary>
        private async Task ChangeState(ChangeEventArgs args)
        {
            // Prevent the select-element of the states from reloading, as this would deselect the state
            stateChanged = true;


            // A state has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                selState = await uow.StateRepo.GetModelByPkAsync(Convert.ToInt32(args.Value));

                cities = await uow.CityRepo.GetAllModelsAsync(Convert.ToInt32(args.Value));
                cities = cities.OrderBy(c => c.CityName).ToList();
            }
            // The option "Please select a state" has been selected
            else
            {
                cities.Clear();
                selState = null;
            }


            // Update the view
            StateHasChanged();
        }



        /// <summary>
        /// If a city is beeing selected, redirect to the edit-view of this city
        /// </summary>
        private void ChangeCity(ChangeEventArgs args)
        {
            // A city has been selected (0 = "Please select a city")
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && selCountry != null && selState != null)
            {
                nav.NavigateTo("/city/edit/" + selCountry.CountryCode + "/" + selState.StateId.ToString() + "/" + args.Value.ToString());
            }
        }
        #endregion
    }
}
