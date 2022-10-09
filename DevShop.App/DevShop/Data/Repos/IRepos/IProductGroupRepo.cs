using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for ProductGroup-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IProductGroupRepo
	{
		void CreateNewProductGroupAsync(ProductGroup productGroup);
		void DeleteProductGroupAsync(int groupNr, string compCode);
		Task<List<ProductGroup>> GetAllProductGroupsAsync();
		Task<ProductGroup> GetProductGroupByIdAsync(int groupNr, string compCode);
		void UpdateProductGroupAsync(ProductGroup productGroup);
	}
}
