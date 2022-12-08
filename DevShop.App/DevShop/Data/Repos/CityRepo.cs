using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type City
	/// </summary>
	public class CityRepo : ICityRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public CityRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Cities from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type City
		/// </returns>
		public async Task<List<City>> GetAllModelsAsync()
		{
			List<City> models = await _context.Cities.ToListAsync();

			return models;
		}



		/// <summary>
		/// Get all existing Cities of a given State from the database
		/// </summary>
		/// <param name="_stateID">
		/// PK of the State, to which the city belongs
		/// </param>
		/// <returns>
		/// A list of objects of type City
		/// </returns>
		public async Task<List<City>> GetAllModelsAsync(int _stateID)
		{
			List<City> models = await _context.Cities.Where(m => m.StateId == _stateID).ToListAsync();

			return models;
		}



		/// <summary>
		/// Get a single City from the database
		/// </summary>
		/// <param name="_stateID">
		/// The ID of the State, to which the City belongs
		/// </param>
		/// <param name="_zip">
		/// The ZIP of the City
		/// </param>
		/// <returns>
		/// A single object of type City
		/// </returns>
		public async Task<City> GetModelByPkAsync(int _stateID, string _zip)
		{
			City model = await _context.Cities.FirstOrDefaultAsync(m => m.StateId == _stateID && m.Zip == _zip);

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
		public async Task CreateModelAsync(City _model)
		{
			_context.Cities.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(City _model)
		{
			_context.Cities.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_stateID">
		/// The ID of the State, to which the City belongs
		/// </param>
		/// <param name="_zip">
		/// The ZIP of the City
		/// </param>
		public async Task DeleteModelAsync(int _stateID, string _zip)
		{
			City model = await GetModelByPkAsync(_stateID, _zip);

			if (model != null)
			{
				_context.Cities.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
