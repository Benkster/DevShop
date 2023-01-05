using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Company-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface ICompanyRepo
	{
		Task CreateModelAsync(Company _model);
		Task DeleteModelAsync(string _pk);
		Task<List<Company>> GetAllModelsAsync();
		Task<List<Company>> GetAllModelsAsync(bool _onlyProducer);
		Task<Company> GetModelByPkAsync(string _pk);
        Task<string> GetNextPkAsync(string _countryCode);
        Task UpdateModelAsync(Company _model);
	}
}
