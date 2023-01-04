using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Unit
	/// </summary>
	public class UnitRepo : IUnitRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public UnitRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Units from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type Unit
		/// </returns>
		public async Task<List<Unit>> GetAllModelsAsync()
		{
			List<Unit> models = await _context.Units.ToListAsync();

			return models;
		}



		/// <summary>
		/// Get a single Unit from the database
		/// </summary>
		/// <param name="_pk">
		/// The Primary Key of the object in the database
		/// </param>
		/// <returns>
		/// A single object of type Unit
		/// </returns>
		public async Task<Unit> GetModelByPkAsync(string _pk)
		{
			Unit model = await _context.Units.FirstOrDefaultAsync(m => m.UnitCode == _pk);

			return model;
		}
		#endregion



		#region Create/Update
		/// <summary>
		/// Create a new entry in the database
		/// </summary>
		/// <param name="_model">
		/// Data of the model
		/// </param>
		public async Task CreateModelAsync(Unit _model)
		{
			_context.Units.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Unit _model)
		{
			_context.Units.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_pk">
		/// Primary Key of the model, that should be removed
		/// </param>
		public async Task DeleteModelAsync(string _pk)
		{
			Unit model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.Units.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
