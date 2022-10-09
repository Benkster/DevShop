using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Product-objects
	/// </summary>
	public class ProductRepo : IProductRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public ProductRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Products from the DB
		/// </summary>
		/// <returns>
		/// A list of all Products
		/// </returns>
		public async Task<List<Product>> GetAllProductsAsync()
		{
			List<Product> products = new List<Product>();


			products = await _context.Products.ToListAsync();


			return products;
		}



		/// <summary>
		/// Get the selected Product from the DB via its number and the number of the correspronding ProductGroup
		/// </summary>
		/// <param name="prodNr">
		/// The number of the Product
		/// </param>
		/// <param name="groupNr">
		/// The number of the ProductGroup, to which the Product belongs
		/// </param>
		/// <returns>
		/// A single object of type Product
		/// </returns>
		public async Task<Product> GetProductByIdAsync(int prodNr, int groupNr)
		{
			Product product = new Product();


			product = await _context.Products.FirstOrDefaultAsync(p => p.ProductNr == prodNr && p.ProductGroupNr == groupNr);


			return product;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Product into the DB
		/// </summary>
		/// <param name="product">
		/// The Product, that should be created
		/// </param>
		public async void CreateNewProductAsync(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Product
		/// </summary>
		/// <param name="product">
		/// The Product, that should be updated, containing the new data
		/// </param>
		public async void UpdateProductAsync(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Product from the DB
		/// </summary>
		/// <param name="prodNr">
		/// The number of the Product
		/// </param>
		/// <param name="groupNr">
		/// The number of the ProductGroup, to which the Product belongs
		/// </param>
		public async void DeleteProductAsync(int prodNr, int groupNr)
		{
			Product product = await GetProductByIdAsync(prodNr, groupNr);

			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
