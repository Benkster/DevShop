using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Category-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface ICategoryRepo
	{
		void CreateNewCategoryAsync(Category category);
		void DeleteCategoryAsync(int id);
		Task<List<Category>> GetAllCategoriesAsync();
		Task<Category> GetCategoryByIdAsync(int id);
		void UpdateCategoryAsync(Category category);
	}
}
