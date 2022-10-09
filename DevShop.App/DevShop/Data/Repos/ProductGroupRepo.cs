using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of ProductGroup-objects
	/// </summary>
	public class ProductGroupRepo : IProductGroupRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public ProductGroupRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing ProductGroups from the DB
		/// </summary>
		/// <returns>
		/// A list of all ProductGroups
		/// </returns>
		public async Task<List<ProductGroup>> GetAllProductGroupsAsync()
		{
			List<ProductGroup> productGroups = new List<ProductGroup>();


			productGroups = await _context.ProductGroups.ToListAsync();


			return productGroups;
		}



		/// <summary>
		/// Get the selected ProductGroup from the DB via its number and the code of the corresponding Company
		/// </summary>
		/// <param name="groupNr">
		/// The number of the ProductGroup
		/// </param>
		/// <param name="compCode">
		/// The code of the Company, to which the ProductGroup belongs
		/// </param>
		/// <returns>
		/// A single object of type ProductGroup
		/// </returns>
		public async Task<ProductGroup> GetProductGroupByIdAsync(int groupNr, string compCode)
		{
			ProductGroup productGroup = new ProductGroup();


			productGroup = await _context.ProductGroups.FirstOrDefaultAsync(pg => pg.ProductGroupNr == groupNr && pg.CompCode == compCode);


			return productGroup;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new ProductGroup into the DB
		/// </summary>
		/// <param name="productGroup">
		/// The ProductGroup, that should be created
		/// </param>
		public async void CreateNewProductGroupAsync(ProductGroup productGroup)
		{
			_context.ProductGroups.Add(productGroup);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing ProductGroup
		/// </summary>
		/// <param name="productGroup">
		/// The ProductGroup, that should be updated, containing the new data
		/// </param>
		public async void UpdateProductGroupAsync(ProductGroup productGroup)
		{
			_context.ProductGroups.Update(productGroup);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a ProductGroup from the DB
		/// </summary>
		/// <param name="groupNr">
		/// The number of the ProductGroup
		/// </param>
		/// <param name="compCode">
		/// The code of the Company, to which the ProductGroup belongs
		/// </param>
		public async void DeleteProductGroupAsync(int groupNr, string compCode)
		{
			ProductGroup productGroup = await GetProductGroupByIdAsync(groupNr, compCode);

			_context.ProductGroups.Remove(productGroup);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
