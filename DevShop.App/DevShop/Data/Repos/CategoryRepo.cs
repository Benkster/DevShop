using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
    /// <summary>
    /// Handles data for objects of type Category
    /// </summary>
    public class CategoryRepo : ICategoryRepo
    {
        #region Variables/Properties
        private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public CategoryRepo(DevShopContext context)
        {
            _context = context;
        }
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Categories from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type Category
		/// </returns>
		public async Task<List<Category>> GetAllModelsAsync()
        {
            List<Category> models = await _context.Categories.ToListAsync();

            return models;
        }



        /// <summary>
        /// Get a single Category from the database
        /// </summary>
        /// <param name="_pk">
        /// The Primary Key of the object in the database
        /// </param>
        /// <returns>
        /// A single object of type Category
        /// </returns>
        public async Task<Category> GetModelByPkAsync(int _pk)
        {
            Category model = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == _pk);

            return model;
        }
        #endregion


        #region Create/Update
        /// <summary>
        /// Create a new entry in the database
        /// </summary>
        /// <param name="_category">
        /// Data of the model
        /// </param>
        public async Task CreateModelAsync(Category _category)
        {
            _context.Categories.Add(_category);
            await _context.SaveChangesAsync();
        }



        /// <summary>
        /// Update an existing entry in the database
        /// </summary>
        /// <param name="_category">
        /// New data of the model
        /// </param>
        public async Task UpdateModelAsync(Category _category)
        {
            _context.Categories.Update(_category);
            await _context.SaveChangesAsync();
        }
        #endregion


        #region Delete
        /// <summary>
        /// Remove an entry from the database
        /// </summary>
        /// <param name="_pk">
        /// Primary Key of the model, that should be removed
        /// </param>
        public async Task DeleteModelAsync(int _pk)
        {
            Category category = await GetModelByPkAsync(_pk);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
        #endregion
    }
}
