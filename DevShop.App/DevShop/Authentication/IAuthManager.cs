using DevShop.Data.Models;
using DevShop.Authentication.ViewModels;

namespace DevShop.Authentication
{
	/// <summary>
	/// Interface for the AuthManager.
	/// Use this interface in other classes, to register a new user or log a user in
	/// </summary>
	public interface IAuthManager
	{
		Task<string> LoginAsync(LoginVM loginData);
		Task<(bool success, Dictionary<string, string> errorMessages)> RegisterAsync(User user);
	}
}
