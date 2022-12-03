using DevShop.Data.Models;
using DevShop.Data.SortTypes;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using DevShop.Data.ViewModels.TreeBuilderVMs;

namespace DevShop.Data.Repos
{
    /// <summary>
    /// Handles data for objects of type Category
    /// </summary>
    public class CategoryRepo : ICategoryRepo
    {
        #region Variables/Properties
        private readonly DevShopContext _context;
        private readonly TreeBuilder _treeBuilder;
		#endregion



        #region Constructors
        public CategoryRepo(DevShopContext context, TreeBuilder treeBuilder)
        {
            _context = context;
            _treeBuilder = treeBuilder;
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
        /// Get all existing Categories ordered from the database
        /// </summary>
        /// <param name="sortType">
        /// Specifies the column after which the models should be sorted
        /// </param>
        /// <param name="descending">
        /// If true, sorts models in descending order
        /// </param>
        /// <returns>
        /// An ordered list of objects of type Category
        /// </returns>
        public async Task<List<Category>> GetAllModelsAsync(CategorySortType.SortType sortType, bool descending = false)
        {
            List<Category> models = new List<Category>();

            switch (sortType)
            {
                // Sort by ID
                case CategorySortType.SortType.ID:
                    models = (descending) ? await _context.Categories.OrderByDescending(m => m.CategoryId).ToListAsync() : await _context.Categories.OrderBy(m => m.CategoryId).ToListAsync();
                    break;
                // Sort by Name
                case CategorySortType.SortType.Name:
                    models = (descending) ? await _context.Categories.OrderByDescending(m => m.CategoryName).ToListAsync() : await _context.Categories.OrderBy(m => m.CategoryName).ToListAsync();
                    break;
            }


            return models;
        }



        /// <summary>
        /// Exclude all the children of a specified element
        /// </summary>
        /// <param name="_rootID">
        /// ID of the element, whose child-elements should be excluded from the list
        /// </param>
        /// <returns>
        /// A list of elements excluding all children of the given element
        /// </returns>
        public async Task<List<Category>> GetModelsWithoutChildrenAsync(int _rootID)
        {
            // List of all existing elements
            List<Category> allModels = await _context.Categories.ToListAsync();

            // Convert the type of the list of all elements to SelectListDataVM, which is a general model that is used for the methods of the TreeBuilder
            List<SelectListDataVM> allSelectListModels = allModels.Select(m => new SelectListDataVM()
            {
                ElemID = m.CategoryId,
                ElemName = m.CategoryName,
                ParentID = m.ParentId
            }).ToList();


            // Exclude all children of the given element and get a list of all allowed elements
            List<SelectListDataVM> resultSelectList = _treeBuilder.ExcludeChildrenFromSelectList(allSelectListModels, _rootID);

            // Convert the type of the result-list back to the type of the model, that is used in the view
            List<Category> modelsWithoutChildren = allModels.Where(m => !resultSelectList.All(r => r.ElemID != m.CategoryId)).ToList();


            return modelsWithoutChildren;
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
