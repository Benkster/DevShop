using DevShop.Data.Repos.IRepos;
using DevShop.Models;
using Microsoft.EntityFrameworkCore;

namespace DevShop.Data.Repos
{
	/// <summary>
	/// Contains all important methods for handling the data of Article-objects
	/// </summary>
	public class ArticleRepo : IArticleRepo
	{
		#region Variables/Properties
		private DevShopDbContext _context;
		#endregion



		#region Constructors
		public ArticleRepo(DevShopDbContext context)
		{
			_context = context;
		}
		#endregion



		#region Methods
		#region Select
		/// <summary>
		/// Get all existing Articles from the DB
		/// </summary>
		/// <returns>
		/// A list of all Articles
		/// </returns>
		public async Task<List<Article>> GetAllArticlesAsync()
		{
			List<Article> articles = new List<Article>();


			articles = await _context.Articles.ToListAsync();


			return articles;
		}



		/// <summary>
		/// Get the selected Article from the DB via its number and the number of the corresponding Product
		/// </summary>
		/// <param name="artNr">
		/// The number of the Article
		/// </param>
		/// <param name="prodNr">
		/// The number of the Product, to which the article belongs
		/// </param>
		/// <returns>
		/// A single object of type Article
		/// </returns>
		public async Task<Article> GetArticleByIdAsync(int artNr, int prodNr)
		{
			Article article = new Article();


			article = await _context.Articles.FirstOrDefaultAsync(a => a.ArticleNr == artNr && a.ProductNr == prodNr);


			return article;
		}
		#endregion



		#region Insert/Update
		/// <summary>
		/// Insert a new Article into the DB
		/// </summary>
		/// <param name="article">
		/// The Article, that should be created
		/// </param>
		public async void CreateNewArticleAsync(Article article)
		{
			_context.Articles.Add(article);
			await _context.SaveChangesAsync();
		}



		/// <summary>
		/// Update the data of an existing Article
		/// </summary>
		/// <param name="article">
		/// The Article, that should be updated, containing the new data
		/// </param>
		public async void UpdateArticleAsync(Article article)
		{
			_context.Articles.Update(article);
			await _context.SaveChangesAsync();
		}
		#endregion



		#region Delete
		/// <summary>
		/// Delete a Article from the DB
		/// </summary>
		/// <param name="artNr">
		/// The number of the Article
		/// </param>
		/// <param name="prodNr">
		/// The number of the Product, to which the article belongs
		/// </param>
		public async void DeleteArticleAsync(int artNr, int prodNr)
		{
			Article article = await GetArticleByIdAsync(artNr, prodNr);

			_context.Articles.Remove(article);
			await _context.SaveChangesAsync();
		}
		#endregion
		#endregion
	}
}
