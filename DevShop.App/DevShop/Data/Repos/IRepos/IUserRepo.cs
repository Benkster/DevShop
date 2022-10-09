using DevShop.Models;

namespace DevShop.Data.Repos.IRepos
{
	/// <summary>
	/// Interface for User-Repository
	/// Defines necessary methods, that must be implemented
	/// </summary>
	public interface IUserRepo
	{
		void CreateNewUserAsync(User user);
		void DeleteUserAsync(string id);
		Task<List<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(string id);
		void UpdateUserAsync(User user);
	}
}
