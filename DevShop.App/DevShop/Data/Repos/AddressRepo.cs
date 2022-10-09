using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Address-objects
	/// </summary>
	public class AddressRepo : IAddressRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public AddressRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Addresses from the DB
		/// </summary>
		/// <returns>
		/// A list of all Addresses
		/// </returns>
		public async Task<List<Address>> GetAllAddressesAsync()
		{
			List<Address> addresses = new List<Address>();


			addresses = await _context.Addresses.ToListAsync();


			return addresses;
		}



		/// <summary>
		/// Get the selected Address from the DB via its ID
		/// </summary>
		/// <param name="id">
		/// The ID of the Address
		/// </param>
		/// <returns>
		/// A single object of type Address
		/// </returns>
		public async Task<Address> GetAddressByIdAsync(int id)
		{
			Address address = new Address();


			address = await _context.Addresses.FirstOrDefaultAsync(a => a.AddressID == id);


			return address;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Address into the DB
		/// </summary>
		/// <param name="address">
		/// The Address, that should be created
		/// </param>
		public async void CreateNewAddressAsync(Address address)
		{
			_context.Addresses.Add(address);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Address
		/// </summary>
		/// <param name="address">
		/// The Address, that should be updated, containing the new data
		/// </param>
		public async void UpdateAddressAsync(Address address)
		{
			_context.Addresses.Update(address);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Address from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the Address, that should be deleted
		/// </param>
		public async void DeleteAddressAsync(int id)
		{
			Address address = await GetAddressByIdAsync(id);

			_context.Addresses.Remove(address);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
