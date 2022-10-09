using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for State-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IStateRepo
	{
		void CreateNewStateAsync(State state);
		void DeleteStateAsync(int id);
		Task<List<State>> GetAllStatesAsync();
		Task<State> GetStateByIdAsync(int id);
		void UpdateStateAsync(State state);
	}
}
