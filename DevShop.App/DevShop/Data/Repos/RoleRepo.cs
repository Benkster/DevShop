using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Role
	/// </summary>
	public class RoleRepo : IRoleRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public RoleRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Roles from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type Role
		/// </returns>
		public async Task<List<Role>> GetAllModelsAsync()
		{
			List<Role> model = await _context.Roles.ToListAsync();

			return model;
		}



		/// <summary>
		/// Get a single Role from the database
		/// </summary>
		/// <param name="_pk">
		/// The PK of the Role, that should be selected
		/// </param>
		/// <returns>
		/// A single object of type Role
		/// </returns>
		public async Task<Role> GetModelByPkAsync(int _pk)
		{
			Role model = await _context.Roles.FirstOrDefaultAsync(m => m.RoleNr == _pk);

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
		public async Task CreateModelAsync(Role _model)
		{
			_context.Roles.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Role _model)
		{
			_context.Roles.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_pk">
		/// The PK of the Role, that should be selected
		/// </param>
		public async Task DeleteModelAsync(int _pk)
		{
			Role model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.Roles.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
