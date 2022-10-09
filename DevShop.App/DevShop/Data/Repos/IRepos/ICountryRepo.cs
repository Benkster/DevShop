using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Country-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface ICountryRepo
	{
		void CreateNewCountryAsync(Country country);
		void DeleteCountryAsync(int id, string code);
		Task<List<Country>> GetAllCountriesAsync();
		Task<Country> GetCountryByIdAsync(int id, string code);
		void UpdateCountryAsync(Country country);
	}
}
