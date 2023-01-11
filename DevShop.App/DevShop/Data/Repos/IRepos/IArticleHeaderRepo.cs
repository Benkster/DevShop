using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Article-Header-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IArticleHeaderRepo
	{
		Task CreateModelAsync(ArticleHeader _model);
		Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _prodNr);
		Task<ArticleHeader> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr);
		Task UpdateModelAsync(ArticleHeader _model);
	}
}
