using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Country-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface ICountryRepo
	{
		Task CreateModelAsync(Country _model);
		Task DeleteModelAsync(string _pk);
		Task<List<Country>> GetAllModelsAsync();
		Task<Country> GetModelByPkAsync(string _pk);
		Task UpdateModelAsync(Country _model);
	}
}
