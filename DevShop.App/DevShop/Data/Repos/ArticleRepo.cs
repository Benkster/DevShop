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
		#endregion
	}
}
