using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Product-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IProductRepo
	{
		void CreateNewProductAsync(Product product);
		void DeleteProductAsync(int prodNr, int groupNr);
		Task<List<Product>> GetAllProductsAsync();
		Task<Product> GetProductByIdAsync(int prodNr, int groupNr);
		void UpdateProductAsync(Product product);
	}
}
