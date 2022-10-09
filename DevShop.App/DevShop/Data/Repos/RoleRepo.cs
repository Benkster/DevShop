using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Role-objects
	/// </summary>
	public class RoleRepo : IRoleRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public RoleRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Roles from the DB
		/// </summary>
		/// <returns>
		/// A list of all Roles
		/// </returns>
		public async Task<List<Role>> GetAllRolesAsync()
		{
			List<Role> roles = new List<Role>();


			roles = await _context.Roles.ToListAsync();


			return roles;
		}



		/// <summary>
		/// Get the selected Role from the DB via its ID
		/// </summary>
		/// <param name="id">
		/// The ID of the Role
		/// </param>
		/// <returns>
		/// A single object of type Role
		/// </returns>
		public async Task<Role> GetRoleByIdAsync(int id)
		{
			Role role = new Role();


			role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleNr == id);


			return role;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Role into the DB
		/// </summary>
		/// <param name="role">
		/// The Role, that should be created
		/// </param>
		public async void CreateNewRoleAsync(Role role)
		{
			_context.Roles.Add(role);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Role
		/// </summary>
		/// <param name="role">
		/// The Role, that should be updated, containing the new data
		/// </param>
		public async void UpdateRoleAsync(Role role)
		{
			_context.Roles.Update(role);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Role from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the Role, that should be deleted
		/// </param>
		public async void DeleteRoleAsync(int id)
		{
			Role role = await GetRoleByIdAsync(id);

			_context.Roles.Remove(role);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
