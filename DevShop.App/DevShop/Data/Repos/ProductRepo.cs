using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Product
	/// </summary>
	public class ProductRepo : IProductRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public ProductRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		/// <summary>
		/// Get all existing Products of a given Company from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Products belong
		/// </param>
		/// <returns>
		/// A list of objects of type Product
		/// </returns>
		public async Task<List<Product>> GetAllModelsAsync(string _compCode)
		{
			List<Product> models = await _context.Products.Where(p => p.CompCode == _compCode).ToListAsync();

			return models;
		}



		/// <summary>
		/// Get all existing Products of a given Product-Group
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Products belong
		/// </param>
		/// <param name="_productGroupNr">
		/// The code of the Product-Group, whose Products should be selected
		/// </param>
		/// <returns>
		/// A list of objects of type Product
		/// </returns>
		public async Task<List<Product>> GetAllModelsAsync(string _compCode, int _productGroupNr)
		{
			List<Product> models = await _context.Products.Where(p => p.CompCode == _compCode && p.ProductGroupNr == _productGroupNr).ToListAsync();

			return models;
		}
		#endregion
	}
}
