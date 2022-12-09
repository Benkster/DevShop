using DevShop.Data.Models;

namespace DevShop.Data.Repos.IRepos
{
    /// <summary>
    /// Interface for State-Repository.
    /// Defines all methods, that are necessary for the repo
    /// </summary>
    public interface IStateRepo
    {
        Task CreateModelAsync(State _model);
        Task DeleteModelAsync(int _pk);
        Task<List<State>> GetAllModelsAsync();
        Task<List<State>> GetAllModelsAsync(string _countryCode);
        Task<State> GetModelByPkAsync(int _pk);
        Task UpdateModelAsync(State _model);
    }
}
