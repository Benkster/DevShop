using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Roles
{
    /// <summary>
    /// Class for the Role-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
    public partial class RoleEditView
    {
        #region Variables/Properties
        // Determines, whether an existing model is beeing edited or a new one is beeing created
        private bool isEdit;
        // Indicates, whether the user has selected a different role yet or not
        private bool roleChanged = false;

        // An error message that is beeing shown, if the submition of the form was invalid
        private string errorMessage;

        // The role, that is beeing created/edited
        private Role role;

        // A list of all existing roles
        private List<DropDownVM> dropDownRoles;



        // PK of the role, that is beeing edited
        [Parameter]
        public string RoleNr { get; set; }
        #endregion



        #region Main Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of the OnInitializedAsync, because when the user selects a different role (which changes the parameters of the URL), the displayed data in the form has to update.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            // Get all roles and sort them by Role-Nr
            List<Role> roles = await uow.RoleRepo.GetAllModelsAsync();
            dropDownRoles = roles.OrderBy(r => r.RoleNr).Select(r => new DropDownVM()
            {
                Value = r.RoleNr.ToString(),
                DisplayName = r.RoleName
            }).ToList();


            // Reset the error-message
            errorMessage = string.Empty;


            // An existing role is beeing edited
            if (!string.IsNullOrEmpty(RoleNr))
            {
                isEdit = true;

                role = await uow.RoleRepo.GetModelByPkAsync(Convert.ToInt32(RoleNr));
            }
            // A new role is beeing created
            else
            {
                isEdit = false;

                role = new Role();
            }


            // Update the data on the view
            StateHasChanged();
        }



        /// <summary>
        /// Create a new role or updates an existing one
        /// </summary>
        private async Task Save()
        {
            // Update an existing role
            if (isEdit)
            {
                await uow.RoleRepo.UpdateModelAsync(role);
                nav.NavigateTo("/role");
            }
            // Create a new role
            else
            {
                // Check, whether a role with the given Role-Nr already exists, or not
                Role existingRole = await uow.RoleRepo.GetModelByPkAsync(role.RoleNr);

                // A role with the given Role-Nr already exists
                if (existingRole != null)
                {
                    errorMessage = "A role with this Role-Nr already exists.\nPlease select a different number.";
                }
                // A role with the given Role-Nr does not exist yet
                else
                {
                    await uow.RoleRepo.CreateModelAsync(role);
                    nav.NavigateTo("/role");
                }
            }
        }



        /// <summary>
        /// Delete a role from the database
        /// </summary>
        private async Task Delete()
        {
            // Only delete it, if the role is beeing edited and not created
            if (isEdit)
            {
                // Check, whether this role is assigned to any users, or not
                List<User> roleUsers = await uow.UserRepo.GetAllModelsAsync(Convert.ToInt32(RoleNr));


                // The role is assigned to a user
                if (roleUsers.Count > 0)
                {
                    errorMessage = "This role can not be deleted, due to it beeing assigned to one or more users.";
                }
                // The role is not assigned to any users
                else
                {
                    await uow.RoleRepo.DeleteModelAsync(Convert.ToInt32(RoleNr));
                    nav.NavigateTo("/role");
                }
            }
        }



        /// <summary>
        /// Show an error message, if the form-submit was invalid
        /// </summary>
        private void Error()
        {
            errorMessage = "Something went wrong. Please try again";
        }
        #endregion



        #region Side Methods
        /// <summary>
        /// If the user selects a different role, redirect to the edit-view
        /// </summary>
        private async Task ChangeRole(ChangeEventArgs args)
        {
            // Prevent the select-element of the roles from reloading, as this would deselect the role
            roleChanged = true;


            // Only proceed, if an existing role has been selected (0 = "Please select a role")
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                nav.NavigateTo("/role/edit/" + args.Value.ToString());
            }
        }
        #endregion
    }
}
