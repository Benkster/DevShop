using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Role-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IRoleRepo
	{
		Task<List<Role>> GetAllModelsAsync();
		Task<Role> GetModelByPkAsync(int _pk);
	}
}
