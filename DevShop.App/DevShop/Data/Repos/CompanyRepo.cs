using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	public class CompanyRepo : ICompanyRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public CompanyRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		public async Task<List<Company>> GetAllModelsAsync()
		{
			List<Company> result = await _context.Companies.OrderBy(m => m.CompName).ToListAsync();
			
			return result;
		}



		public async Task<Company> GetModelByPkAsync(string pk)
		{
			Company company = await _context.Companies.FirstOrDefaultAsync(m => m.CompCode == pk);

			return company;
		}
		#endregion
	}
}
