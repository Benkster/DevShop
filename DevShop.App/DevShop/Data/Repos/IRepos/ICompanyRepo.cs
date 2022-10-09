using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Company-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface ICompanyRepo
	{
		void CreateNewCompanyAsync(Company company);
		void DeleteCompanyAsync(string compCode);
		Task<List<Company>> GetAllCompaniesAsync();
		Task<Company> GetCompanyByIdAsync(string compCode);
		void UpdateCompanyAsync(Company company);
	}
}
