using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
    /// <summary>
    /// Interface for Category-Repository.
    /// Defines all methods, that are necessary for the repo
    /// </summary>
    public interface ICategoryRepo
    {
        Task CreateModelAsync(Category _category);
        Task DeleteModelAsync(int _pk);
        Task<List<Category>> GetAllModelsAsync();
        Task<Category> GetModelByPkAsync(int _pk);
        Task UpdateModelAsync(Category _category);
    }
}
