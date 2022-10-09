using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Article-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IArticleRepo
	{
		void CreateNewArticleAsync(Article article);
		void DeleteArticleAsync(int artNr, int prodNr);
		Task<List<Article>> GetAllArticlesAsync();
		Task<Article> GetArticleByIdAsync(int artNr, int prodNr);
		void UpdateArticleAsync(Article article);
	}
}
