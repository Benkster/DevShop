using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Authentication
{
	public partial class RegisterView
	{
		#region Variables
		private string errorMessage = string.Empty;

		private User user;
		private Company company;
		private Role role;

		private List<Company> companies;
		#endregion



		#region Main-Methods
		protected override async Task OnInitializedAsync()
		{
			user = new User();
			role = await uow.RoleRepo.GetModelByPkAsync(1);

			companies = await uow.CompanyRepo.GetAllModelsAsync();

			company = companies.FirstOrDefault();
		}



		private async Task Save()
		{
			user.CompCode = company.CompCode;
			user.RoleNr = role.RoleNr;

			var result = await auth.RegisterAsync(user);
			

			if (result.success)
			{
				// Log user in and navigate to homepage of shop
			}
			else
			{
				// Show error messages
			}
		}



		private async Task Error()
		{
			errorMessage = "Something went wrong. Please reload and try again";
		}
		#endregion



		#region Side-Methods
		private async void ChangeCompany(ChangeEventArgs args)
		{
			company = await uow.CompanyRepo.GetModelByPkAsync(args.Value.ToString());
		}
		#endregion
	}
}
