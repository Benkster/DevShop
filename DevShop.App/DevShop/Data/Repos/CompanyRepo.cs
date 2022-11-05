using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Company-objects
	/// </summary>
	public class CompanyRepo : ICompanyRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public CompanyRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Companies from the DB
		/// </summary>
		/// <returns>
		/// A list of all Companies
		/// </returns>
		public async Task<List<Company>> GetAllCompaniesAsync()
		{
			List<Company> companies = new List<Company>();


			companies = await _context.Companies.ToListAsync();


			return companies;
		}



		/// <summary>
		/// Get the selected Company from the DB via its code
		/// </summary>
		/// <param name="compCode">
		/// The code of the Company
		/// </param>
		/// <returns>
		/// A single object of type Company
		/// </returns>
		public async Task<Company> GetCompanyByIdAsync(string compCode)
		{
			Company company = new Company();


			company = await _context.Companies.FirstOrDefaultAsync(c => c.CompCode == compCode);


			return company;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Company into the DB
		/// </summary>
		/// <param name="company">
		/// The Company, that should be created
		/// </param>
		public async Task CreateNewCompanyAsync(Company company)
		{
			_context.Companies.Add(company);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Company
		/// </summary>
		/// <param name="company">
		/// The Company, that should be updated, containing the new data
		/// </param>
		public async Task UpdateCompanyAsync(Company company)
		{
			_context.Companies.Update(company);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Company from the DB
		/// </summary>
		/// <param name="compCode">
		/// Code of the Company, that should be deleted
		/// </param>
		public async Task DeleteCompanyAsync(string compCode)
		{
			Company company = await GetCompanyByIdAsync(compCode);

			_context.Companies.Remove(company);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
