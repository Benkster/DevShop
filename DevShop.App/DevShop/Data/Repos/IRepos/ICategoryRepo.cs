using DevShop.Data.Models;
using DevShop.Data.SortTypes;

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
        Task<List<Category>> GetAllModelsAsync(CategorySortType.SortType sortType, bool descending = false);
        Task<Category> GetModelByPkAsync(int _pk);
        Task<List<Category>> GetModelsWithoutChildrenAsync(int _rootID);
        Task UpdateModelAsync(Category _category);
    }
}
