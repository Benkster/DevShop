using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Country-objects
	/// </summary>
	public class CountryRepo : ICountryRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public CountryRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Countries from the DB
		/// </summary>
		/// <returns>
		/// A list of all Countries
		/// </returns>
		public async Task<List<Country>> GetAllCountriesAsync()
		{
			List<Country> countries = new List<Country>();


			countries = await _context.Countries.ToListAsync();


			return countries;
		}



		/// <summary>
		/// Get the selected Country from the DB via its ID and code
		/// </summary>
		/// <param name="id">
		/// The ID of the Country
		/// </param>
		/// <param name="code">
		/// The code of the Country (e.g. 'AT')
		/// </param>
		/// <returns>
		/// A single object of type Country
		/// </returns>
		public async Task<Country> GetCountryByIdAsync(int id, string code)
		{
			Country country = new Country();


			country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryID == id && c.CountryCode == code);


			return country;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Country into the DB
		/// </summary>
		/// <param name="country">
		/// The Country, that should be created
		/// </param>
		public async void CreateNewCountryAsync(Country country)
		{
			_context.Countries.Add(country);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Country
		/// </summary>
		/// <param name="country">
		/// The Country, that should be updated, containing the new data
		/// </param>
		public async void UpdateCountryAsync(Country country)
		{
			_context.Countries.Update(country);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Country from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the Country, that should be deleted
		/// </param>
		/// <param name="code">
		/// Code of the Country, that should be deleted (e.g. 'AT')
		/// </param>
		public async void DeleteCountryAsync(int id, string code)
		{
			Country country = await GetCountryByIdAsync(id, code);

			_context.Countries.Remove(country);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
