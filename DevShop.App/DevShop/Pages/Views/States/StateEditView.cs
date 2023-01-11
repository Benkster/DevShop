using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Eventing.Reader;

namespace DevShop.Pages.Views.States
{
    /// <summary>
    /// Class for the State-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
    public partial class StateEditView
    {
        #region Variables/Properties
        // Determines, whether a new country is beeing created or an existing one is beeing updated
        private bool isEdit;

        // This error-message is shown, if the submittion of the form failed/was invalid
        private string errorMessage;

        // Represents the selected country in the select-element of all countries
        private Country country;
        // Represents the country, to which the selected state/the new state belongs
        private Country selCountry;
        // The state, that is beeing edited/created
        private State state;

        // All existing countries
        private List<Country> countries;
        // All states of the selected country
        private List<DropDownVM> dropDownStates;



        // The PK of the country, to which the state (that is beeing edited) belongs
        [Parameter]
        public string CountryCode { get; set; }

        // The PK of the state, that is beeing edited
        [Parameter]
        public string StateID { get; set; }
        #endregion



        #region Main Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of OnInitializedAsync, because when the user selects an different state (which changes the parameters of the URL), the displayed data in the form has to update.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            // Get all countries from the database and sort them by name
            countries = await uow.CountryRepo.GetAllModelsAsync();
            countries = countries.OrderBy(c => c.CountryName).ToList();


            // An existing state is beeing edited
            if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(StateID))
            {
                isEdit = true;

                state = await uow.StateRepo.GetModelByPkAsync(Convert.ToInt32(StateID));
                country = await uow.CountryRepo.GetModelByPkAsync(CountryCode);
                selCountry = country;

                List<State> states = await uow.StateRepo.GetAllModelsAsync(CountryCode);
                dropDownStates = states.OrderBy(s => s.StateName).Select(s => new DropDownVM()
                {
                    Value = s.StateId.ToString(),
                    DisplayName = s.StateName
                }).ToList();
            }
            // A new state is beeing created
            else
            {
                isEdit = false;

                state = new State();
                selCountry = countries.FirstOrDefault();
            }


            StateHasChanged();
        }



        /// <summary>
        /// Create a new state in the database or update an existing one
        /// </summary>
        private async Task Save()
        {
            // Specify the country, to which this state belongs
            state.CountryId = selCountry.CountryId;
            state.CountryCode = selCountry.CountryCode;


            // Update an existing state
            if (isEdit)
            {
                await uow.StateRepo.UpdateModelAsync(state);
            }
            // Create a new state
            else
            {
                await uow.StateRepo.CreateModelAsync(state);
            }


            // Return to the overview
            nav.NavigateTo("/state/" + CountryCode);
        }



        /// <summary>
        /// The form-submition was invalid
        /// </summary>
        private async Task Error()
        {
            errorMessage = "Something went wrong. Please try again";
        }



        /// <summary>
        /// Delete a state from the database
        /// </summary>
        private async Task Delete()
        {
            // Only delete it, if a state is beeing edited and not created
            if (isEdit)
            {
                await uow.StateRepo.DeleteModelAsync(Convert.ToInt32(StateID));
                nav.NavigateTo("/state/" + CountryCode);
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
                nav.NavigateTo("/state/" + args.Value.ToString());
            }
            // If no existing country has been selected, return to the overview without selecting a country
            else
            {
                nav.NavigateTo("/state");
            }
        }



        /// <summary>
        /// Change the state, that is beeing edited
        /// </summary>
        private void ChangeState(ChangeEventArgs args)
        {
            // If the state, that has been selected, exists, edit it
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && !string.IsNullOrEmpty(CountryCode))
            {
                nav.NavigateTo("/state/edit/" + CountryCode + "/" + args.Value.ToString());
            }
            // No existing state has been selected -> return to the overview and select the country, that has been selected in the edit-view
            else if (!string.IsNullOrEmpty(CountryCode))
            {
                nav.NavigateTo("/state/" + CountryCode);
            }
            // No state and no country are given -> return to the overview without selecting a country
            else
            {
                nav.NavigateTo("/state");
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
            }
        }
        #endregion
    }
}
