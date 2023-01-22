using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics.Eventing.Reader;

namespace DevShop.Pages.Views.Authentication
{
	/// <summary>
	/// Class for the User-Edit-View/Register-View.
	/// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
	/// </summary>
	public partial class RegisterView
	{
        #region Variables/Properties
        // Determines, whether a new model is beeing created or an existing one is beeing updated
        private bool isEdit;

        // This error-message is shown, if the submittion of the form failed/was invalid
        private string errorMessage;

        // Represents the selected company in the select-element of all companies
        private Company company;
        // Represents the company, to which the selected user/the new user belongs
        private Company selCompany;
        // Represents the role, that is assigned to the user
        private Role selRole;
        // The user, that is beeing edited/created
        private User user;

        // All existing companies
        private List<Company> companies;
        // All existing roles
        private List<Role> roles;
        // All users of the selected company
        private List<DropDownVM> dropDownUsers;

        // A collection of error-messages, if the registration of a new user failed
        private Dictionary<string, string> registerErrors;



        // The PK of the company, to which the user (that is beeing edited) belongs
        [Parameter]
        public string CompCode { get; set; }

        // The PK of the user, that is beeing edited
        [Parameter]
        public string UserId { get; set; }
        #endregion



        #region Main Methods
        /// <summary>
        /// Called, when the parameters of the URL change.
        /// This is needed instead of OnInitializedAsync, because when the user selects a different user (which changes the parameters of the URL), the displayed data in the form has to update.
        /// </summary>
        protected override async Task OnParametersSetAsync()
        {
            // Get all companies from the database and sort them by name
            companies = await uow.CompanyRepo.GetAllModelsAsync();
            companies = companies.OrderBy(c => c.CompName).OrderBy(c => c.CompCode.Substring(0, 2)).ToList();

            // Get all roles from the database and sort them by Role-Nr
            roles = await uow.RoleRepo.GetAllModelsAsync();
            roles = roles.OrderBy(r => r.RoleNr).ToList();

            // Reset the collection of registration-errors
            registerErrors = new Dictionary<string, string>();


            // An existing user is beeing edited
            if (!string.IsNullOrEmpty(CompCode) && !string.IsNullOrEmpty(UserId))
            {
                isEdit = true;

                user = await uow.UserRepo.GetModelByPkAsync(Convert.ToInt32(UserId));
                company = await uow.CompanyRepo.GetModelByPkAsync(CompCode);
                selCompany = company;
                selRole = await uow.RoleRepo.GetModelByPkAsync(user.RoleNr);

                List<User> users = await uow.UserRepo.GetAllModelsAsync(CompCode);
                dropDownUsers = users.OrderBy(u => u.LastName).Select(u => new DropDownVM()
                {
                    Value = u.UserId.ToString(),
                    DisplayName = u.LastName + " " + u.FirstName
                }).ToList();
            }
            // A new user is beeing created
            else
            {
                isEdit = false;

                user = new User();
                selCompany = companies.FirstOrDefault();
                selRole = roles.FirstOrDefault();
            }


            StateHasChanged();
        }



        /// <summary>
        /// Create a new user in the database or update an existing one
        /// </summary>
        private async Task Save()
        {
            // Specify the company, to which this user belongs
            user.CompCode = selCompany.CompCode;
            // Assign a role to the user
            user.RoleNr = selRole.RoleNr;


            // Update an existing user
            if (isEdit)
            {
                await uow.UserRepo.UpdateModelAsync(user);
                nav.NavigateTo("/user/" + CompCode);
            }
            // Create a new user
            else
            {
                var result = await auth.RegisterAsync(user);

                // Registration succeeded
                if (result.success)
                {
                    nav.NavigateTo("/user/" + user.CompCode);
                }
                // Registration failed (due to invalid data)
                else
                {
                    registerErrors = result.errorMessages;

                    StateHasChanged();
                }
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
        /// Delete a user from the database
        /// </summary>
        private async Task Delete()
        {
            // Only delete it, if a user is beeing edited and not created
            if (isEdit)
            {
                // Get all discounts of the user
                List<UserDiscount> userDiscounts = await uow.UserDiscountRepo.GetAllModelsAsync(Convert.ToInt32(UserId));

                // Delete all discounts that the user had
                if (userDiscounts.Count > 0)
                {
                    foreach (UserDiscount discount in userDiscounts)
                    {
                        await uow.UserDiscountRepo.DeleteModelAsync(discount.UserDiscountId);
                    }
                }


                // Delete the user
                await uow.UserRepo.DeleteModelAsync(Convert.ToInt32(UserId));
                nav.NavigateTo("/user/" + CompCode);
            }
        }
        #endregion



        #region Side Methods
        /// <summary>
        /// Redirect to the overview, if the user changes the selected company.
        /// The newly selected company will be selected in overview as well.
        /// </summary>
        private void ChangeCompany(ChangeEventArgs args)
        {
            // Select the company in the overview as well, if an existing company has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
            {
                nav.NavigateTo("/user/" + args.Value.ToString());
            }
            // If no existing company has been selected, return to the overview without selecting a country
            else
            {
                nav.NavigateTo("/user");
            }
        }



        /// <summary>
        /// Change the user, that is beeing edited
        /// </summary>
        private void ChangeUser(ChangeEventArgs args)
        {
            // If the user, that has been selected, exists, edit it
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0" && !string.IsNullOrEmpty(CompCode))
            {
                nav.NavigateTo("/user/edit/" + CompCode + "/" + args.Value.ToString());
            }
            // No existing user has been selected -> return to the overview and select the company, that has been selected in the edit-view
            else if (!string.IsNullOrEmpty(CompCode))
            {
                nav.NavigateTo("/user/" + CompCode);
            }
            // No user and no company are given -> return to the overview without selecting a company
            else
            {
                nav.NavigateTo("/user");
            }
        }



        /// <summary>
        /// Stores the information, to which company the user belongs
        /// </summary>
        private async Task SetCompany(ChangeEventArgs args)
        {
            // Only store the information, if an existing company has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selCompany = await uow.CompanyRepo.GetModelByPkAsync(args.Value.ToString());
            }
        }



        /// <summary>
        /// Stores the information, which role is beeing assigned to the user
        /// </summary>
        private async Task SetRole(ChangeEventArgs args)
        {
            // Only store the information, if an existing role has been selected
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                selRole = await uow.RoleRepo.GetModelByPkAsync(Convert.ToInt32(args.Value));
            }
        }
        #endregion
    }
}
