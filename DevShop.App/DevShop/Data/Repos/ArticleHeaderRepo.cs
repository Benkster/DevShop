using DevShop.Data.Models;
using DevShop.Data.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Handles data for objects of type ArticleHeader
	/// </summary>
	public class ArticleHeaderRepo : IArticleHeaderRepo
	{
		#region Variables
		private readonly DevShopContext _context;
		#endregion



		#region Constructors
		public ArticleHeaderRepo(DevShopContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Get
		/// <summary>
		/// Get a single Article-Header from the database
		/// </summary>
		/// <param name="_compCode">
		/// Code of the Company, to which the Article-Header belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// Number of the Product-Group, to which the Article-Header belongs
		/// </param>
		/// <param name="_prodNr">
		/// Number of the Product, to which the Article-Header belongs
		/// </param>
		/// <returns>
		/// A single object of type ArticleHeader
		/// </returns>
		public async Task<ArticleHeader> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr)
		{
			ArticleHeader model = await _context.ArticleHeaders.FirstOrDefaultAsync(m => m.CompCode == _compCode && m.ProductGroupNr == _prodGroupNr && m.ProductNr == _prodNr);


			if (model == null)
			{
				model = new ArticleHeader();
			}


			return model;
		}
		#endregion


		#region Create/Update
		/// <summary>
		/// Create a new entry in the database
		/// </summary>
		/// <param name="_model">
		/// Data of the model
		/// </param>
		public async Task CreateModelAsync(ArticleHeader _model)
		{
			_context.ArticleHeaders.Add(_model);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update an existing entry in the database
		/// </summary>
		/// <param name="_model">
		/// New data of the model
		/// </param>
		public async Task UpdateModelAsync(ArticleHeader _model)
		{
			_context.ArticleHeaders.Update(_model);
			await _context.SaveChangesAsync();
		}
		#endregion


		#region Delete
		/// <summary>
		/// Remove an entry from the database
		/// </summary>
		/// <param name="_compCode">
		/// Code of the Company, to which the Article-Header belongs
		/// </param>
		/// <param name="_prodGroupNr">
		/// Number of the Product-Group, to which the Article-Header belongs
		/// </param>
		/// <param name="_prodNr">
		/// Number of the Product, to which the Article-Header belongs
		/// </param>
		public async Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _prodNr)
		{
			ArticleHeader model = await GetModelByPkAsync(_compCode, _prodGroupNr, _prodNr);

			if (model != null)
			{
				_context.ArticleHeaders.Remove(model);
				await _context.SaveChangesAsync();
			}
		}
		#endregion
		#endregion
	}
}
