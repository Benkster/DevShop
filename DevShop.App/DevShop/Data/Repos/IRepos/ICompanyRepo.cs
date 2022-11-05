using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Company-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface ICompanyRepo
	{
		Task CreateNewCompanyAsync(Company company);
		Task DeleteCompanyAsync(string compCode);
		Task<List<Company>> GetAllCompaniesAsync();
		Task<Company> GetCompanyByIdAsync(string compCode);
		Task UpdateCompanyAsync(Company company);
	}
}
