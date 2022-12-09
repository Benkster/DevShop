using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Roles
{
    /// <summary>
    /// Class for the Role-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
    public partial class RoleView
    {
        #region Variables
        // List of all roles
        List<Role> roles;
        #endregion



        #region Main Methods
        /// <summary>
        /// Called automatically, when the page loads.
        /// Sets variables needed for the view.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            roles = await uow.RoleRepo.GetAllModelsAsync();
        }
        #endregion



        #region Side Methdos
        /// <summary>
        /// If the user selects a role, redirect to the edit-view
        /// </summary>
        private void ChangeRole(ChangeEventArgs args)
        {
            // Only proceed, if an existing role has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                nav.NavigateTo("/role/edit/" + args.Value.ToString());
            }
        }
        #endregion
    }
}
