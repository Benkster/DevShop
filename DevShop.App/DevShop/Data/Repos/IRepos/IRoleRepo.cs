using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Role-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IRoleRepo
	{
		void CreateNewRoleAsync(Role role);
		void DeleteRoleAsync(int id);
		Task<List<Role>> GetAllRolesAsync();
		Task<Role> GetRoleByIdAsync(int id);
		void UpdateRoleAsync(Role role);
	}
}
