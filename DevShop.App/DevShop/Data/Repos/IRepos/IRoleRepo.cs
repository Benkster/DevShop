using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	public interface IRoleRepo
	{
		Task<List<Role>> GetAllModelsAsync();
		Task<Role> GetModelViaPk(int pk);
	}
}
