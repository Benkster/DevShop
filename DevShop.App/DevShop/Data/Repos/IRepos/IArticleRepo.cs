using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Article-Repository.
	/// Defines all methods, that are necessary for the repo
	/// </summary>
	public interface IArticleRepo
	{
		Task CreateModelAsync(Article _model);
		Task DeleteModelAsync(string _compCode, int _prodGroupNr, int _productNr, int _articleNr);
		Task<List<Article>> GetAllModelsAsync(string _compCode);
		Task<Article> GetModelByPkAsync(string _compCode, int _prodGroupNr, int _prodNr, int _artNr);
		Task<int> GetNextPkAsync(string _compCode);
		Task UpdateModelAsync(Article _model);
	}
}
