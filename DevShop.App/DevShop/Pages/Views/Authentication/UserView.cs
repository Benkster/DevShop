using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Authentication
{
    /// <summary>
    /// Class for the User-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
    public partial class UserView
    {
        #region Variables
        // Indicates, whether the user has selected a company yet or not
        private bool companyChanged = false;

        // Stores the data of the selected company
        private Company selCompany;

        // A list of all companies
        private List<Company> companies;
        // A list of all users from the selected company
        private List<User> users;



        // The PK of a company (if the user changes the selected company in the edit-view, he will be redirected here and the company he selected should also be selected here)
        [Parameter]
        public string CompCode { get; set; }
        #endregion



        #region Main Mehtods
        /// <summary>
        /// Called automatically, when the page loads.
        /// Sets variables needed for the view.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Get all companies from the database and sort them by name
            companies = await uow.CompanyRepo.GetAllModelsAsync();
            companies = companies.OrderBy(c => c.CompName).OrderBy(c => c.CompCode.Substring(0, 2)).ToList();


            // The user changed the selected company in the edit-view
            if (!string.IsNullOrEmpty(CompCode))
            {
                // Get the company, that the user selected in the edit-view
                selCompany = await uow.CompanyRepo.GetModelByPkAsync(CompCode);

                // Get all users of the company, that has been selected in the edit-view, and sort them by their last name
                users = await uow.UserRepo.GetAllModelsAsync(CompCode);
                users = users.OrderBy(u => u.LastName).ToList();
            }
        }
        #endregion



        #region Side Methods
        /// <summary>
        /// Changes the selected company, which also reloads all users, that belong to the newly selected company
        /// </summary>
        private async Task ChangeCompany(ChangeEventArgs args)
        {
            // Prevent the select-element of the companies from reloading, as this would deselect the company
            companyChanged = true;


            // A company has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                selCompany = await uow.CompanyRepo.GetModelByPkAsync(args.Value.ToString());

                users = await uow.UserRepo.GetAllModelsAsync(args.Value.ToString());
                users = users.OrderBy(u => u.LastName).ToList();
            }
            // The option "Please select a company" has been selected
            else
            {
                users.Clear();
                selCompany = null;
            }


            // Update the view
            StateHasChanged();
        }



        /// <summary>
        /// If a user is beeing selected, redirect to the edit-view of this user
        /// </summary>
        private void ChangeUser(ChangeEventArgs args)
        {
            // A user has been selected (0 = "Please select a user")
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && selCompany != null)
            {
                nav.NavigateTo("/user/edit/" + selCompany.CompCode + "/" + args.Value.ToString());
            }
        }
        #endregion
    }
}
