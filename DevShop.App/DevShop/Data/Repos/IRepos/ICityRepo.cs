using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for City-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface ICityRepo
	{
		Task CreateModelAsync(City _model);
		Task DeleteModelAsync(int _stateID, string _zip);
		Task<List<City>> GetAllModelsAsync(int _stateID);
		Task<List<City>> GetAllModelsAsync();
		Task<City> GetModelByPkAsync(int _stateID, string _zip);
		Task UpdateModelAsync(City _model);
	}
}
