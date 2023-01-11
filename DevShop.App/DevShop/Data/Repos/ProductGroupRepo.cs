using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using DevShop.Data.ViewModels.TreeBuilderVMs;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type ProductGroup
	/// </summary>
	public class ProductGroupRepo : IProductGroupRepo
    {
		#region Variables
		private DevShopContext _context;
		private readonly TreeBuilder _treeBuilder;
		#endregion



		#region Constructors
		public ProductGroupRepo(DevShopContext context, TreeBuilder treeBuilder)
		{
			_context = context;
			_treeBuilder = treeBuilder;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing ProductGroups from the database
		/// </summary>
		/// <returns>
		/// A list of objects of type ProductGroup
		/// </returns>
		public async Task<List<ProductGroup>> GetAllModelsAsync()
		{
			List<ProductGroup> model = await _context.ProductGroups.ToListAsync();

			return model;
		}




		/// <summary>
		/// Get all existing ProductGroups of a given Company from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the ProducGroups belong
		/// </param>
		/// <returns>
		/// A list of objects of type ProductGroup
		/// </returns>
		public async Task<List<ProductGroup>> GetAllModelsAsync(string _compCode)
		{
			List<ProductGroup> model = await _context.ProductGroups.Where(m => m.CompCode == _compCode).ToListAsync();

			return model;
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
		public async Task<List<ProductGroup>> GetModelsWithoutChildrenAsync(int _rootID, string _compCode)
		{
			// List of all existing elements
			List<ProductGroup> allModels = await _context.ProductGroups.Where(m => m.CompCode == _compCode).ToListAsync();

			// Convert the type of the list of all elements to SelectListDataVM, which is a general model that is used for the methods of the TreeBuilder
			List<SelectListDataVM> allSelectListModels = allModels.Select(m => new SelectListDataVM()
			{
				ElemID = m.ProductGroupNr,
				ElemName = m.GroupName,
				ParentID = m.ParentId
			}).ToList();


			// Exclude all children of the given element and get a list of all allowed elements
			List<SelectListDataVM> resultSelectList = _treeBuilder.ExcludeChildrenFromSelectList(allSelectListModels, _rootID);

			// Convert the type of the result-list back to the type of the model, that is used in the view
			List<ProductGroup> modelsWithoutChildren = allModels.Where(m => !resultSelectList.All(r => r.ElemID != m.ProductGroupNr)).ToList();


			return modelsWithoutChildren;
		}



		/// <summary>
		/// Get a single Company from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the ProductGroup belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup
		/// </param>
		/// <returns>
		/// A single object of type Company
		/// </returns>
		public async Task<ProductGroup> GetModelByPkAsync(string _compCode, int _prodGroupNr)
		{
			ProductGroup model = await _context.ProductGroups.FirstOrDefaultAsync(m => m.ProductGroupNr == _prodGroupNr && m.CompCode == _compCode);

			return model;
		}



		/// <summary>
		/// Get the next available number of a Product-Group of a given Company
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Product-Group belongs
		/// </param>
		/// <returns>
		/// An integer conatining the next available number for the Product-Group
		/// </returns>
		public async Task<int> GetNextPkAsync(string _compCode)
		{
			// Indicates the next available PK
			int nextPk = 1;

			// Get the Product-Group, with the highest Group-Nr in the given Company
			ProductGroup model = await _context.ProductGroups.Where(m => m.CompCode == _compCode).OrderByDescending(m => m.ProductGroupNr).FirstOrDefaultAsync();


			// Check, whether a Product-Group exists for the given Company
			if (model != null)
			{
				// Count up the highest Group-Nr
				nextPk = model.ProductGroupNr + 1;
			}


			return nextPk;
		}
		#endregion



		#region Create/Update
		/// <summary>
		/// Create a new entry in the database
		/// </summary>
		/// <param name="_model">
		/// Data of the model
		/// </param>
		public async Task CreateModelAsync(ProductGroup _model)
		{
			_context.ProductGroups.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(ProductGroup _model)
		{
			_context.ProductGroups.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the ProductGroup belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup
		/// </param>
		public async Task DeleteModelAsync(string _compCode, int _prodGroupNr)
		{
			ProductGroup model = await GetModelByPkAsync(_compCode, _prodGroupNr);

			if (model != null)
			{
				_context.ProductGroups.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
