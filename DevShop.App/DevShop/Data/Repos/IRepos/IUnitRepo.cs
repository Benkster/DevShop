using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for Unit-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IUnitRepo
	{
		void CreateNewUnitAsync(Unit unit);
		void DeleteUnitAsync(string code);
		Task<List<Unit>> GetAllUnitsAsync();
		Task<Unit> GetUnitByIdAsync(string code);
		void UpdateUnitAsync(Unit unit);
	}
}
