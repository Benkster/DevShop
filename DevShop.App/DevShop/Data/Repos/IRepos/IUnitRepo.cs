using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Unit-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IUnitRepo
	{
		Task CreateModelAsync(Unit _model);
		Task DeleteModelAsync(string _pk);
		Task<List<Unit>> GetAllModelsAsync();
		Task<Unit> GetModelByPkAsync(string _pk);
		Task UpdateModelAsync(Unit _model);
	}
}
