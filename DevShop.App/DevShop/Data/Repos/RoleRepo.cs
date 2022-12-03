using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
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
		public async Task<List<Role>> GetAllModelsAsync()
		{
			List<Role> result = await _context.Roles.ToListAsync();

			return result;
		}



		public async Task<Role> GetModelViaPk(int pk)
		{
			Role role = await _context.Roles.FirstOrDefaultAsync(m => m.RoleNr == pk);

			return role;
		}
		#endregion
	}
}
