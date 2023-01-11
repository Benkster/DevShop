using DevShop.Data.Models;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Units
{
    /// <summary>
    /// Class for the Unit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class UnitView
	{
		#region Variables
		private List<Unit> units;
		#endregion



		#region Main Methods
		/// <summary>
		/// Called automatically, when the page loads.
		/// Sets all variables that are used in the view
		/// </summary>
		/// <returns></returns>
		protected override async Task OnInitializedAsync()
		{
			units = await uow.UnitRepo.GetAllModelsAsync();
			units = units.OrderBy(u => u.UnitCode).ToList();
		}
		#endregion



		#region Side Methods
		/// <summary>
		/// If the user changes the selected Unit, redirect to the edit-view
		/// </summary>
		private void ChangeUnit(ChangeEventArgs args)
		{
			// Only proceed, if an existing unit has been selected (0 = "Please select a Unit")
			if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
			{
				nav.NavigateTo("/unit/edit/" + args.Value.ToString());
			}
		}
		#endregion
	}
}
