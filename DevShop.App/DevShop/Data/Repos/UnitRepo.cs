using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Unit-objects
	/// </summary>
	public class UnitRepo : IUnitRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public UnitRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Units from the DB
		/// </summary>
		/// <returns>
		/// A list of all Units
		/// </returns>
		public async Task<List<Unit>> GetAllUnitsAsync()
		{
			List<Unit> units = new List<Unit>();


			units = await _context.Units.ToListAsync();


			return units;
		}



		/// <summary>
		/// Get the selected Unit from the DB via its code
		/// </summary>
		/// <param name="code">
		/// The code of the Unit
		/// </param>
		/// <returns>
		/// A single object of type Unit
		/// </returns>
		public async Task<Unit> GetUnitByIdAsync(string code)
		{
			Unit unit = new Unit();


			unit = await _context.Units.FirstOrDefaultAsync(u => u.UnitCode == code);


			return unit;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Unit into the DB
		/// </summary>
		/// <param name="unit">
		/// The Unit, that should be created
		/// </param>
		public async void CreateNewUnitAsync(Unit unit)
		{
			_context.Units.Add(unit);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Unit
		/// </summary>
		/// <param name="unit">
		/// The Unit, that should be updated, containing the new data
		/// </param>
		public async void UpdateUnitAsync(Unit unit)
		{
			_context.Units.Update(unit);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Unit from the DB
		/// </summary>
		/// <param name="code">
		/// The code of the Unit
		/// </param>
		public async void DeleteUnitAsync(string code)
		{
			Unit unit = await GetUnitByIdAsync(code);

			_context.Units.Remove(unit);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
