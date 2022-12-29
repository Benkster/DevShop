using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Product-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IProductRepo
	{
		Task<List<Product>> GetAllModelsAsync(string _compCode);
		Task<List<Product>> GetAllModelsAsync(string _compCode, int _productGroupNr);
	}
}
