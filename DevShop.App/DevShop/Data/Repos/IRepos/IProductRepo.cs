using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Product-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IProductRepo
	{
		Task CreateModelAsync(Product _model);
		Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _productNr);
		Task<List<Product>> GetAllModelsAsync(string _compCode);
		Task<List<Product>> GetAllModelsAsync(string _compCode, int _productGroupNr);
		Task<Product> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr);
		Task<int> GetNextPkAsync(string _compCode);
		Task UpdateModelAsync(Product _model);
	}
}
