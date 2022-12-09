using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Company
	/// </summary>
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
		#region Get
		/// <summary>
		/// Get all existing Companies from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type Company
		/// </returns>
		public async Task<List<Company>> GetAllModelsAsync()
		{
			List<Company> model = await _context.Companies.ToListAsync();
			
			return model;
		}



		/// <summary>
		/// Get a single Company from the database
		/// </summary>
		/// <param name="_pk">
		/// The Primary Key of the object in the database
		/// </param>
		/// <returns>
		/// A single object of type Company
		/// </returns>
		public async Task<Company> GetModelByPkAsync(string _pk)
		{
			Company model = await _context.Companies.FirstOrDefaultAsync(m => m.CompCode == _pk);

			return model;
		}



		/// <summary>
		/// Get the next available CompCode in the given Country
		/// </summary>
		/// <param name="_countryCode">
		/// A two-character code of the Country, the Company resides in
		/// </param>
		/// <returns>
		/// A string conatining the next available CompCode
		/// </returns>
		public async Task<string> GetNextPkAsync(string _countryCode)
        {
			// Holds the next available ContryCode
			string nextPk = string.Empty;

			// Get the company with the highest CompCode from the given country
			Company model = await _context.Companies.Where(c => c.CompCode.Substring(0, 2) == _countryCode).OrderByDescending(m => m.CompCode).FirstOrDefaultAsync();


			// Count up the highest CompCode by 1 or if no company in this country has been created yet, set 1 as the first number of the CompCode
			int nextNum = (model == null) ? 1 : Convert.ToInt32(model.CompCode.Substring(2, 4)) + 1;

			string nextNumText = nextNum.ToString();
			string fillZeros = string.Empty;


			// The CompCode has to consist of 4 digits. If the next highest number is < 1000, fill the rest up with zeros at the start of the CompCode
			for (int count = nextNumText.Length; count < 4; count++)
            {
				fillZeros += "0";
            }


			// Add the CountryCode, the filled up zeros and the new highest number together, to form the new CompCode
			nextPk = _countryCode + fillZeros + nextNumText;


			return nextPk;
        }
		#endregion



		#region Create/Update
		/// <summary>
		/// Create a new entry in the database
		/// </summary>
		/// <param name="_model">
		/// Data of the model
		/// </param>
		public async Task CreateModelAsync(Company _model)
		{
			_context.Companies.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Company _model)
		{
			_context.Companies.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_pk">
		/// Primary Key of the model, that should be removed
		/// </param>
		public async Task DeleteModelAsync(string _pk)
		{
			Company model = await GetModelByPkAsync(_pk);

			if (model != null)
			{
				_context.Companies.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
