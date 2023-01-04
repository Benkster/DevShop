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



		/// <summary>
		/// Get a single Product from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Product belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup, to which the Product belongs
		/// </param>
		/// <param name="_prodNr">
		/// The number of the Product
		/// </param>
		/// <returns>
		/// A single object of type Product
		/// </returns>
		public async Task<Product> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr)
		{
			Product model = await _context.Products.FirstOrDefaultAsync(m => m.CompCode == _compCode && m.ProductGroupNr == _prodGroupNr && m.ProductNr == _prodNr);

			return model;
		}



		/// <summary>
		/// Get the next available number of a Product of a given Company
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Product belongs
		/// </param>
		/// <returns>
		/// An integer conatining the next available number for the Product
		/// </returns>
		public async Task<int> GetNextPkAsync(string _compCode)
		{
			// Indicates the next available PK
			int nextPk = 1;

			// Get the Product with the highest Prod-Nr in the given Company
			Product model = await _context.Products.Where(m => m.CompCode == _compCode).OrderByDescending(m => m.ProductNr).FirstOrDefaultAsync();


			// Check, whether a Product exists for the given Company
			if (model != null)
			{
				// Count up the highest Prod-Nr
				nextPk = model.ProductNr + 1;
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
		public async Task CreateModelAsync(Product _model)
		{
			_context.Products.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Product _model)
		{
			_context.Products.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Product belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup, to which the Product belongs
		/// </param>
		/// <param name="_productNr">
		/// The number of the Product
		/// </param>
		public async Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _productNr)
		{
			Product model = await GetModelByPkAsync(_compCode, _prodGroupNr, _productNr);

			if (model != null)
			{
				_context.Products.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
	}
}
