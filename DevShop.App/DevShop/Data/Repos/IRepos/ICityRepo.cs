using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for City-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface ICityRepo
	{
		void CreateNewCityAsync(City city);
		void DeleteCityAsync(int stateID, string zip);
		Task<List<City>> GetAllCitiesAsync();
		Task<City> GetCityByIdAsync(int stateID, string zip);
		void UpdateCityAsync(City city);
	}
}
