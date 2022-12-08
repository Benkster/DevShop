using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Cities
{
    /// <summary>
    /// Class for the City-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class CityEditView
    {
        #region Variables/Properties
        // Determines, whether a new city is beeing created or an existing one is beeing updated
        private bool isEdit;

        // This error-message is shown, if the submittion of the form failed/was invalid
        private string errorMessage;

        // Represents the selected country in the select-element of all countries
        private Country country;
        // Represents the country, to which the selected city/the new city belongs
        private Country selCountry;
        // Represents the selected state in the select-element of all states
        private State state;
        // Represents the state, to which the selected city/the new city belongs
        private State selState;
        // The city, that is beeing created/edited
        private City city;

        // All existing countries
        private List<Country> countries;
        // All states of the selected country
        private List<State> states;
        // All states of the selected city
        private List<DropDownVM> dropDownStates;
        // All cities of the selected state
        private List<DropDownVM> dropDownCities;



        // The PK of the country, to which the city (that is beeing edited) belongs
        [Parameter]
        public string CountryCode { get; set; }

        // The PK of the state, to which the city (that is beeing edited) belongs
        [Parameter]
        public string StateID { get; set; }

        // The PK of the city, that is beeing edited
		[Parameter]
		public string Zip { get; set; }
        #endregion



        #region Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of OnInitializedAsync, because when the user selects an different city (which changes the parameters of the URL), the displayed data in the form has to update.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            // Get all countries from the database and sort them by name
            countries = await uow.CountryRepo.GetAllModelsAsync();
            countries = countries.OrderBy(c => c.CountryName).ToList();


            // An existing city is beeing edited
            if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(StateID) && !string.IsNullOrEmpty(Zip))
            {
                isEdit = true;

                state = await uow.StateRepo.GetModelByPkAsync(Convert.ToInt32(StateID));
                country = await uow.CountryRepo.GetModelByPkAsync(CountryCode);
                city = await uow.CityRepo.GetModelByPkAsync(Convert.ToInt32(StateID), Zip);

                selState = state;
                selCountry = country;


                states = await uow.StateRepo.GetAllModelsAsync(CountryCode);
                dropDownStates = states.OrderBy(s => s.StateName).Select(s => new DropDownVM()
                {
                    Value = s.StateId.ToString(),
                    DisplayName = s.StateName
                }).ToList();

                List<City> cities = await uow.CityRepo.GetAllModelsAsync(Convert.ToInt32(StateID));
                dropDownCities = cities.OrderBy(c => c.CityName).Select(c => new DropDownVM()
                {
                    Value = c.Zip,
                    DisplayName = c.CityName
                }).ToList();
            }
            // A new city is beeing created
            else
            {
                isEdit = false;

                city = new City();

                
                if (countries.Count > 0)
                {
                    country = countries.FirstOrDefault();
                    selCountry = country;

                    states = await uow.StateRepo.GetAllModelsAsync(countries.FirstOrDefault().CountryCode);

                    state = states.FirstOrDefault();
                    selState = state;

                    dropDownStates = states.OrderBy(s => s.StateName).Select(s => new DropDownVM()
                    {
                        Value = s.StateId.ToString(),
                        DisplayName = s.StateName
                    }).ToList();
                }
            }


            StateHasChanged();
        }



        /// <summary>
        /// Create a new city in the database or update an existing one
        /// </summary>
        private async Task Save()
        {
            if (selState != null)
            {
                // Specify the state, to which this city belongs
                city.StateId = selState.StateId;


                // Update an existing city
                if (isEdit)
                {
                    await uow.CityRepo.UpdateModelAsync(city);
                }
                // Create a new city
                else
                {
                    await uow.CityRepo.CreateModelAsync(city);
                }


                // Return to the overview
                nav.NavigateTo("/city/" + CountryCode);
            }
			else
			{
                errorMessage = "Please select a state";
			}
        }



        /// <summary>
        /// The form-submition was invalid
        /// </summary>
        private async Task Error()
        {
            errorMessage = "Something went wrong. Please try again";
        }



        /// <summary>
        /// Delete a city from the database
        /// </summary>
        private async Task Delete()
        {
            // Only delete it, if a city is beeing edited and not created
            if (isEdit)
            {
                await uow.CityRepo.DeleteModelAsync(Convert.ToInt32(StateID), Zip);
                nav.NavigateTo("/city/" + CountryCode + "/" + StateID);
            }
        }
        #endregion



        #region Side Methods
        /// <summary>
        /// Redirect to the overview, if the user changes the selected country.
        /// The newly selected country will be selected in overview as well.
        /// </summary>
        private void ChangeCountry(ChangeEventArgs args)
        {
            // Select the country in the overview as well, if an existing country has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                nav.NavigateTo("/city/" + args.Value.ToString());
            }
            // If no existing country has been selected, return to the overview without selecting a country
            else
            {
                nav.NavigateTo("/city");
            }
        }



        /// <summary>
        /// Redirect to the overview, if the user changes the selected state.
        /// The newly selected state will be selected in overview as well.
        /// </summary>
        private async void ChangeState(ChangeEventArgs args)
        {
            // Select the country and the state in the overview as well, if an existing country and state have been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && country != null)
            {
                nav.NavigateTo("/city/" + country.CountryCode + "/" + args.Value.ToString());
            }
            // Select the country in the overview as well, if an existing country has been selected
            else if (country != null)
			{
                nav.NavigateTo("/city/" + country.CountryCode);
			}
            // If no existing country or state has been selected, return to the overview without selecting anything
            else
            {
                nav.NavigateTo("/city");
            }
        }



        /// <summary>
        /// Change the city, that is beeing edited
        /// </summary>
        private void ChangeCity(ChangeEventArgs args)
        {
            // If the city, that has been selected, exists, edit it
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && !string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(StateID))
            {
                nav.NavigateTo("/city/edit/" + CountryCode + "/" + StateID + "/" + args.Value.ToString());
            }
            // No existing city has been selected -> return to the overview and select the country and state, that have been selected in the edit-view
            else if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(StateID))
            {
                nav.NavigateTo("/city/" + CountryCode + "/" + StateID);
            }
            // No existing city and no existing state have been selected -> return to the overview and select the country, that has been selected in the edit-view
            else if (!string.IsNullOrEmpty(CountryCode))
			{
                nav.NavigateTo("/city/" + CountryCode);
			}
            // No city, state or country are given -> return to the overview without selecting anything
            else
            {
                nav.NavigateTo("/city");
            }
        }



        /// <summary>
        /// Stores the information, to which country the state belongs
        /// </summary>
        private async Task SetCountry(ChangeEventArgs args)
        {
            // Only store the information, if an existing country has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selCountry = await uow.CountryRepo.GetModelByPkAsync(args.Value.ToString());


                if (selCountry != null)
				{
                    states = await uow.StateRepo.GetAllModelsAsync(selCountry.CountryCode);
                    selState = states.FirstOrDefault();
				}


                StateHasChanged();
            }
        }



        /// <summary>
        /// Stores the information, to which state the city belongs
        /// </summary>
        private async Task SetState(ChangeEventArgs args)
        {
            // Only store the information, if an existing state has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selState = await uow.StateRepo.GetModelByPkAsync(Convert.ToInt32(args.Value));
            }
        }
        #endregion
    }
}
