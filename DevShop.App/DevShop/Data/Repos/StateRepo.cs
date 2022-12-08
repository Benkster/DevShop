using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type State
	/// </summary>
	public class StateRepo : IStateRepo
    {
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public StateRepo(DevShopContext context)
        {
            _context = context;
        }
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing States from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type State
		/// </returns>
		public async Task<List<State>> GetAllModelsAsync()
		{
			List<State> models = await _context.States.ToListAsync();

			return models;
		}



		/// <summary>
		/// Get all existing States of a given Country from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type State
		/// </returns>
		public async Task<List<State>> GetAllModelsAsync(string _countryCode)
        {
			List<State> models = await _context.States.Where(m => m.CountryCode == _countryCode).ToListAsync();

			return models;
        }



		/// <summary>
		/// Get a single State from the database
		/// </summary>
		/// <param name="_pk">
		/// The Primary Key of the object in the database
		/// </param>
		/// <returns>
		/// A single object of type State
		/// </returns>
		public async Task<State> GetModelByPkAsync(int _pk)
		{
			State model = await _context.States.FirstOrDefaultAsync(m => m.StateId == _pk);

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
		public async Task CreateModelAsync(State _model)
		{
			_context.States.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(State _model)
		{
			_context.States.Update(_model);
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
		public async Task DeleteModelAsync(int _pk)
		{
			State model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.States.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
