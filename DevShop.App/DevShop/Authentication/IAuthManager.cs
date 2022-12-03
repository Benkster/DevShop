using DevShop.Data.Models;
using DevShop.Data.ViewModels;

namespace DevShop.Authentication
{
	public interface IAuthManager
	{
		Task<string> LoginAsync(LoginVM loginData);
		Task<(bool success, Dictionary<string, string> errorMessages)> RegisterAsync(User user);
	}
}
