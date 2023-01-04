using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type Article
	/// </summary>
	public class ArticleRepo : IArticleRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public ArticleRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get all existing Articles of a given Company from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belong
		/// </param>
		/// <returns>
		/// A list of objects of type Article
		/// </returns>
		public async Task<List<Article>> GetAllModelsAsync(string _compCode)
		{
			List<Article> models = await _context.Articles.Where(a => a.CompCode == _compCode).ToListAsync();

			return models;
		}



		/// <summary>
		/// Get a single Article from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup, to which the Article belongs
		/// </param>
		/// <param name="_prodNr">
		/// The number of the Product, to which the Article belongs
		/// </param>
		/// <param name="_artNr">
		/// The number of the Article
		/// </param>
		/// <returns>
		/// A single object of type Article
		/// </returns>
		public async Task<Article> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr, int _artNr)
		{
			Article model = await _context.Articles.FirstOrDefaultAsync(m => m.CompCode == _compCode && m.ProductGroupNr == _prodGroupNr && m.ProductNr == _prodNr && m.ArticleNr == _artNr);

			return model;
		}



		/// <summary>
		/// Get the next available number of an Article of a given Company
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belongs
		/// </param>
		/// <returns>
		/// An integer conatining the next available number for the Article
		/// </returns>
		public async Task<int> GetNextPkAsync(string _compCode)
		{
			// Indicates the next available PK
			int nextPk = 1;

			// Get the Article with the highest Art-Nr in the given Company
			Article model = await _context.Articles.Where(m => m.CompCode == _compCode).OrderByDescending(m => m.ArticleNr).FirstOrDefaultAsync();


			// Check, whether an Article exists for the given Company
			if (model != null)
			{
				// Count up the highest Art-Nr
				nextPk = model.ArticleNr + 1;
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
		public async Task CreateModelAsync(Article _model)
		{
			_context.Articles.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(Article _model)
		{
			_context.Articles.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_compCode">
		/// The code of the Company, to which the Article belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// The number of the ProductGroup, to which the Article belongs
		/// </param>
		/// <param name="_productNr">
		/// The number of the Product, to which the Article belongs
		/// </param>
		/// <param name="_articleNr">
		/// The number of the Article
		/// </param>
		public async Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _productNr, int _articleNr)
		{
			Article model = await GetModelByPkAsync(_compCode, _prodGroupNr, _productNr, _articleNr);

			if (model != null)
			{
				_context.Articles.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
