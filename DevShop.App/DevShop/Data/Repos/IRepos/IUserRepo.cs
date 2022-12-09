using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
    /// <summary>
    /// Interface for User-Repository.
    /// Defines all methods, that are necessary for the repo
    /// </summary>
    public interface IUserRepo
    {
        Task<List<User>> GetAllModelsAsync(string _compCode);
    }
}
