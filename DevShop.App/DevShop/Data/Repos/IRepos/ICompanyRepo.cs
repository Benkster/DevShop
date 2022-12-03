using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	public interface ICompanyRepo
	{
		Task<List<Company>> GetAllModelsAsync();
		Task<Company> GetModelByPkAsync(string pk);
	}
}
