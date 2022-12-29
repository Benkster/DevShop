using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Article-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IArticleRepo
	{
		Task<List<Article>> GetAllModelsAsync(string _compCode);
	}
}
