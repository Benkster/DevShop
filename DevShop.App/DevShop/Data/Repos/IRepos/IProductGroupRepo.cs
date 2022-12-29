using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
    /// <summary>
    /// Interface for ProductGroup-Repository.
    /// Defines all methods, that are necessary for the repo
    /// </summary>
    public interface IProductGroupRepo
    {
        Task CreateModelAsync(ProductGroup _model);
        Task DeleteModelAsync(string _compCode, int _prodGroupNr);
        Task<List<ProductGroup>> GetAllModelsAsync();
		Task<List<ProductGroup>> GetAllModelsAsync(string _compCode);
		Task<ProductGroup> GetModelByPkAsync(string _compCode, int _prodGroupNr);
		Task<List<ProductGroup>> GetModelsWithoutChildrenAsync(int _rootID, string _compCode);
		Task<int> GetNextPkAsync(string _compCode);
		Task UpdateModelAsync(ProductGroup _model);
    }
}
