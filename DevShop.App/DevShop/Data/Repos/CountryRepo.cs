using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Country
	/// </summary>
	public class CountryRepo : ICountryRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public CountryRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Countries from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type Country
		/// </returns>
		public async Task<List<Country>> GetAllModelsAsync()
		{
			List<Country> models = await _context.Countries.ToListAsync();

			return models;
		}



		/// <summary>
		/// Get a single Country from the database
		/// </summary>
		/// <param name="_pk">
		/// The Primary Key of the object in the database
		/// </param>
		/// <returns>
		/// A single object of type Country
		/// </returns>
		public async Task<Country> GetModelByPkAsync(string _pk)
		{
			Country model = await _context.Countries.FirstOrDefaultAsync(m => m.CountryCode == _pk);

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
		public async Task CreateModelAsync(Country _model)
		{
			_context.Countries.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Country _model)
		{
			_context.Countries.Update(_model);
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
			Country model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.Countries.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
