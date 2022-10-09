using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of City-objects
	/// </summary>
	public class CityRepo : ICityRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public CityRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Cities from the DB
		/// </summary>
		/// <returns>
		/// A list of all Cities
		/// </returns>
		public async Task<List<City>> GetAllCitiesAsync()
		{
			List<City> cities = new List<City>();


			cities = await _context.Cities.ToListAsync();


			return cities;
		}



		/// <summary>
		/// Get the selected City from the DB via its ID and zip-code
		/// </summary>
		/// <param name="stateID">
		/// The ID of the State, the City belongs to
		/// </param>
		/// <param name="zip">
		/// The zip-code of the City
		/// </param>
		/// <returns>
		/// A single object of type City
		/// </returns>
		public async Task<City> GetCityByIdAsync(int stateID, string zip)
		{
			City city = new City();


			city = await _context.Cities.FirstOrDefaultAsync(c => c.ZIP == zip && c.StateID == stateID);


			return city;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new City into the DB
		/// </summary>
		/// <param name="city">
		/// The City, that should be created
		/// </param>
		public async void CreateNewCityAsync(City city)
		{
			_context.Cities.Add(city);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing City
		/// </summary>
		/// <param name="city">
		/// The City, that should be updated, containing the new data
		/// </param>
		public async void UpdateCityAsync(City city)
		{
			_context.Cities.Update(city);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a City from the DB
		/// </summary>
		/// <param name="stateID">
		/// The ID of the State, the City belongs to
		/// </param>
		/// <param name="zip">
		/// The zip-code of the City
		/// </param>
		public async void DeleteCityAsync(int stateID, string zip)
		{
			City city = await GetCityByIdAsync(stateID, zip);

			_context.Cities.Remove(city);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
