using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Category-objects
	/// </summary>
	public class CategoryRepo : ICategoryRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public CategoryRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Categories from the DB
		/// </summary>
		/// <returns>
		/// A list of all Categories
		/// </returns>
		public async Task<List<Category>> GetAllCategoriesAsync()
		{
			List<Category> categories = new List<Category>();


			categories = await _context.Categories.ToListAsync();


			return categories;
		}



		/// <summary>
		/// Get the selected Category from the DB via its ID
		/// </summary>
		/// <param name="id">
		/// The ID of the Category
		/// </param>
		/// <returns>
		/// A single object of type Category
		/// </returns>
		public async Task<Category> GetCategoryByIdAsync(int id)
		{
			Category category = new Category();


			category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);


			return category;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Category into the DB
		/// </summary>
		/// <param name="category">
		/// The Category, that should be created
		/// </param>
		public async void CreateNewCategoryAsync(Category category)
		{
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Category
		/// </summary>
		/// <param name="category">
		/// The Category, that should be updated, containing the new data
		/// </param>
		public async void UpdateCategoryAsync(Category category)
		{
			_context.Categories.Update(category);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Category from the DB
		/// </summary>
		/// <param name="id">
		/// ID of the Category, that should be deleted
		/// </param>
		public async void DeleteCategoryAsync(int id)
		{
			Category category = await GetCategoryByIdAsync(id);

			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
