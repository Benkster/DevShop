using DevShop.Data.Models;
using DevShop.Data.ViewModels;
using Microsoft.AspNetCore.Components;

namespace DevShop.Pages.Views.Units
{
    /// <summary>
    /// Class for the Unit-Edit-View.
    /// Contains methods and variables, that are necessary for the View or for retrieving data from the Repository
    /// </summary>
	public partial class UnitEditView
	{
		#region Variables/Properties
		// Determines, whether a new unit is beeing created or an existing one is beeing edited
		private bool isEdit;
		// Indicates, whether the user has selected a different unit yet or not
		private bool unitChanged = false;

		// An error message that is beeing shown, if the submition of the form was invalid
		private string errorMessage;

		// The unit, that is beeing created/edited
		private Unit unit;

		// List of all units
		private List<DropDownVM> dropDownUnits;



		// PK of the unit, that is beeing edited
		[Parameter]
		public string UnitCode { get; set; }
		#endregion



		#region Main Methods
		/// <summary>
		/// Called, when the parameters of the URL change.
		/// This is needed instead of the OnInitializedAsync, because when the user selects a different unit (which changes the parameters of the URL), the displayed data in the form has to update.
		/// </summary>
		protected override async Task OnParametersSetAsync()
		{
			// Get all existing units from the DB
			List<Unit> units = await uow.UnitRepo.GetAllModelsAsync();
			dropDownUnits = units.Select(u => new DropDownVM()
			{
				DisplayName = u.UnitName,
				Value = u.UnitCode
			}).OrderBy(d => d.Value).ToList();


			// An existing unit is beeing edited
			if (!string.IsNullOrEmpty(UnitCode))
			{
				isEdit = true;

				unit = await uow.UnitRepo.GetModelByPkAsync(UnitCode);
			}
			// A new unit is beeing created
			else
			{
				isEdit = false;

				unit = new Unit();
			}


			// Update the data on the view
			StateHasChanged();
		}



		/// <summary>
		/// Create a new unit or update an existing one
		/// </summary>
		private async Task Save()
		{
			if (isEdit)
			{
				await uow.UnitRepo.UpdateModelAsync(unit);
			}
			else
			{
				await uow.UnitRepo.CreateModelAsync(unit);
			}


			nav.NavigateTo("/unit");
		}



		/// <summary>
		/// Delete a unit from the database
		/// </summary>
		private async Task Delete()
		{
			// Only delete it, if an existing unit is beeing edited
			if (isEdit)
			{
				await uow.UnitRepo.DeleteModelAsync(UnitCode);

				nav.NavigateTo("/unit");
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
		/// If the user selects a different unit, redirect to the edit-view
		/// </summary>
		private async Task ChangeUnit(ChangeEventArgs args)
		{
			// Prevent the select-element of the units from reloading, as this would deselect the unit
			unitChanged = true;


			// Only proceed, if an existing unit has been selected (0 = "Please select a unit")
			if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString() != "0")
			{
				nav.NavigateTo("/unit/edit/" + args.Value.ToString());
			}
		}
		#endregion
	}
}
