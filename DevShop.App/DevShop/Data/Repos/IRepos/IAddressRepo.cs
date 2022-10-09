using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Address-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IAddressRepo
	{
		void CreateNewAddressAsync(Address address);
		void DeleteAddressAsync(int id);
		Task<Address> GetAddressByIdAsync(int id);
		Task<List<Address>> GetAllAddressesAsync();
		void UpdateAddressAsync(Address address);
	}
}
